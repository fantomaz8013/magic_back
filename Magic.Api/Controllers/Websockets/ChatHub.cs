using System.Collections.Concurrent;
using Magic.Common.Models.Websocket;
using Magic.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets;

[Authorize]
public class ChatHub : Hub
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserService _userService;

    private static readonly ChatHistory chatHistory = new();
    private static readonly Rooms Rooms = new();

    public ChatHub(IServiceProvider serviceProvider, IUserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
    }

    public async Task NewMessage(string message)
    {
        var roomName = Rooms.GetRoom(Context.ConnectionId);
        if (roomName is null)
            throw new HubException("User doesn't belong to any room!");
        var callerUser = await _userService.CurrentUser();
        var chatMessage = new ChatMessage(Guid.NewGuid(), callerUser.Login, message);
        chatHistory.AddMessage(roomName, chatMessage);
        await Clients.Group(roomName).SendAsync("messageReceived", chatMessage);
    }

    public async Task JoinRoom(string roomName)
    {
        Rooms.AddToRoom(Context.ConnectionId, roomName);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        var messages = chatHistory.GetMessages(roomName);
        if (messages.Length > 0)
            await Clients.Caller.SendAsync("historyReceived", messages);
    }

    public async Task LeaveRoom(string roomName)
    {
        Rooms.LeaveRoom(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        if (Rooms.IsRoomEmpty(roomName))
            chatHistory.ClearHistory(roomName);
    }
}

public class ChatHistory
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<ChatMessage>> _messages = new();

    public void AddMessage(string roomName, ChatMessage message)
    {
        _messages.AddOrUpdate(
            roomName,
            key => new ConcurrentBag<ChatMessage> { message },
            (key, oldValue) =>
            {
                oldValue.Add(message);
                return oldValue;
            });
    }

    public ChatMessage[] GetMessages(string roomName)
    {
        return _messages.TryGetValue(roomName, out var bag)
            ? bag.ToArray()
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

public class Rooms
{
    private readonly ConcurrentDictionary<string, string> _connections = new();

    public void AddToRoom(string connectionId, string roomName)
    {
        _connections.AddOrUpdate(connectionId, roomName, (key, oldValue) => roomName);
    }

    public string? GetRoom(string connectionId)
    {
        return _connections.TryGetValue(connectionId, out var roomName)
            ? roomName
            : null;
    }

    public bool LeaveRoom(string connectionId)
    {
        return _connections.TryRemove(connectionId, out _);
    }

    public bool IsRoomEmpty(string roomName)
    {
        return !_connections.Values.Contains(roomName);
    }
}