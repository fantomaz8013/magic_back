using System.Collections.Concurrent;
using Magic.Common.Models.Response;
using Magic.Domain.Enums;
using Magic.Service;
using Magic.Service.Interfaces;
using Magic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets;

[Authorize]
public class GameSessionsHub : Hub
{
    private readonly IUserService _userService;
    private readonly IGameSessionService _gameSessionService;
    private readonly IGameSessionMessageService _gameSessionMessageService;

    private static readonly ConnectedUsers ConnectedUsers = new();

    public GameSessionsHub(IUserService userService, IGameSessionService gameSessionService,
        IGameSessionMessageService gameSessionMessageService)
    {
        _userService = userService;
        _gameSessionService = gameSessionService;
        _gameSessionMessageService = gameSessionMessageService;
    }

    public async Task NewMessage(string message)
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is null)
            throw new HubException("GameSession not found");
        var messageEntity = await _gameSessionMessageService.AddChatMessage(new Guid(gameSessionId), message);
        await Clients.Group(gameSessionId).SendAsync("messageReceived", messageEntity);
    }

    public async Task RollDice(CubeTypeEnum cubeTypeEnum)
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is null)
            throw new HubException("GameSession not found");

        var rollDice = DiceUtil.RollDice(cubeTypeEnum);

        var messageEntity =
            await _gameSessionMessageService.AddDiceMessage(new Guid(gameSessionId), rollDice, cubeTypeEnum);
        await Clients.Group(gameSessionId).SendAsync("messageReceived", messageEntity);
    }

    private async Task CreateServerMessage(string message)
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is null)
            throw new HubException("GameSession not found");
        var messageEntity = await _gameSessionMessageService.AddServerMessage(new Guid(gameSessionId), message);

        await Clients.Group(gameSessionId).SendAsync("messageReceived", messageEntity);
    }

    public async Task JoinGameSession(string gameSessionId)
    {
        var gameSession = await _gameSessionService.GetById(new Guid(gameSessionId));

        if (gameSession is null)
        {
            throw new HubException("GameSession doesn't exists");
        }

        var callerUser = await _userService.CurrentUser();

        if (gameSession.CreatorUserId != callerUser!.Id && gameSession.Users.All(u => u.Id != callerUser.Id))
        {
            throw new HubException("GameSession doesn't exists");
        }

        if (ConnectedUsers.IsConnected(gameSessionId, callerUser!.Id))
        {
            throw new HubException("User already connected to this session!");
        }

        ConnectedUsers.Connect(gameSessionId, callerUser!.Id, Context.ConnectionId);

        await CreateServerMessage($"Player \"{callerUser.Login}\" joined!");

        await Groups.AddToGroupAsync(Context.ConnectionId, gameSessionId);

        var messages = await _gameSessionMessageService.GetMessages(new Guid(gameSessionId));

        if (messages.Count > 0)
            await Clients.Caller.SendAsync("historyReceived", messages);
    }


    public async Task LeaveGameSession(string gameSessionId)
    {
        var callerUser = await _userService.CurrentUser();
        if (!ConnectedUsers.IsConnected(gameSessionId, callerUser!.Id))
        {
            throw new HubException("User not connected to this session");
        }

        await CreateServerMessage($"Player \"{callerUser.Login}\" disconnected");
        ConnectedUsers.Disconnect(gameSessionId, callerUser.Id, Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameSessionId);

        // if (GameSessions.IsGameSessionEmpty(gameSessionId))
        //     chatHistory.ClearHistory(gameSessionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is not null)
            await LeaveGameSession(gameSessionId);

        await base.OnDisconnectedAsync(exception);
    }
}

public class ConnectedUsers
{
    // gameSessionId -> dic<userId, connectionId>
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, string>> _connections = new();

    public void Connect(string gameSessionId, Guid userId, string connectionId)
    {
        _connections.AddOrUpdate(
            gameSessionId,
            key =>
            {
                var dic = new ConcurrentDictionary<Guid, string>();
                dic.TryAdd(userId, connectionId);
                return dic;
            },
            (key, oldValue) =>
            {
                oldValue.TryAdd(userId, connectionId);
                return oldValue;
            });
    }

    public void Disconnect(string gameSessionId, Guid userId, string connectionId)
    {
        if (_connections.TryGetValue(gameSessionId, out var connectedUsers))
            connectedUsers.TryRemove(userId, out _);
    }

    public bool IsConnected(string gameSessionId, Guid userId)
    {
        return _connections.TryGetValue(gameSessionId, out var connectedUsers)
               && connectedUsers.TryGetValue(userId, out _);
    }

    public string? GetGameSessionId(string connectionId)
    {
        var gameSessionIds = _connections
            .Where(c => c.Value.Values.Contains(connectionId))
            .ToList();
        return gameSessionIds.Count == 0 ? null : gameSessionIds[0].Key;
    }
}