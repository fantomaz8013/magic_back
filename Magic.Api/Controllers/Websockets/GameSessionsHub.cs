using System.Collections.Concurrent;
using Magic.Common.Models.Websocket;
using Magic.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets;

[Authorize]
public class GameSessionsHub : Hub
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserService _userService;

    private static readonly ChatHistory chatHistory = new();
    private static readonly GameSessions GameSessions = new();

    public GameSessionsHub(IServiceProvider serviceProvider, IUserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
    }

    public async Task NewMessage(string message)
    {
        var gameSessionId = GameSessions.GetSessionId(Context.ConnectionId);
        if (gameSessionId is null)
            throw new HubException("User doesn't belong to any room!");
        var callerUser = await _userService.CurrentUser();
        var chatMessage = new ChatMessage(Guid.NewGuid(), callerUser.Login, message);
        chatHistory.AddMessage(gameSessionId, chatMessage);
        await Clients.Group(gameSessionId).SendAsync("messageReceived", chatMessage);
    }

    public async Task JoinGameSession(string gameSessionId)
    {
        GameSessions.AddToSession(Context.ConnectionId, gameSessionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, gameSessionId);
        var callerUser = await _userService.CurrentUser();
        await NewMessage($"Player \"{callerUser.Login}\" joined!");
        var messages = chatHistory.GetMessages(gameSessionId);
        if (messages.Length > 0)
            await Clients.Caller.SendAsync("historyReceived", messages);
    }

    public async Task LeaveGameSession(string gameSessionId)
    {
        GameSessions.LeaveSession(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameSessionId);
        if (GameSessions.IsGameSessionEmpty(gameSessionId))
            chatHistory.ClearHistory(gameSessionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomName = GameSessions.GetSessionId(Context.ConnectionId);
        if (roomName is not null)
            await LeaveGameSession(roomName);

        await base.OnDisconnectedAsync(exception);
    }
}

public class ChatHistory
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>> _messages = new();

    public void AddMessage(string gameSessionId, ChatMessage message)
    {
        _messages.AddOrUpdate(
            gameSessionId,
            key =>
            {
                var queue = new ConcurrentQueue<ChatMessage>();
                queue.Enqueue(message);
                return queue;
            },
            (key, oldValue) =>
            {
                oldValue.Enqueue(message);
                return oldValue;
            });
    }

    public ChatMessage[] GetMessages(string gameSessionId)
    {
        return _messages.TryGetValue(gameSessionId, out var chatMessages)
            ? chatMessages.ToArray()
            : Array.Empty<ChatMessage>();
    }

    public void ClearHistory(string roomName)
    {
        if (_messages.TryRemove(roomName, out var bag))
        {
            bag.Clear();
        }
    }
}

public class GameSessions
{
    private readonly ConcurrentDictionary<string, string> _connections = new();

    public void AddToSession(string connectionId, string gameSessionId)
    {
        _connections.AddOrUpdate(connectionId, gameSessionId, (key, oldValue) => gameSessionId);
    }

    public string? GetSessionId(string connectionId)
    {
        return _connections.TryGetValue(connectionId, out var roomName)
            ? roomName
            : null;
    }

    public bool LeaveSession(string connectionId)
    {
        return _connections.TryRemove(connectionId, out _);
    }

    public bool IsGameSessionEmpty(string gameSessionId)
    {
        return !_connections.Values.Contains(gameSessionId);
    }
}