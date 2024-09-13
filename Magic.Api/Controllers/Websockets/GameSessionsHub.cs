using Magic.Api.Controllers.Websockets.InMemory;
using Magic.Api.Controllers.Websockets.Requests;
using Magic.Api.Controllers.Websockets.Responses;
using Magic.Common.Models.Response;
using Magic.Domain.Enums;
using Magic.Service;
using Magic.Service.Interfaces;
using Magic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets;

/// <summary>
/// В этом файле описываются КОР механики хаба, отправка сообщений, подклечение, броски кубика и т.д
/// </summary>
[Authorize]
partial class GameSessionsHub : Hub
{
    private readonly IUserService _userService;
    private readonly IGameSessionService _gameSessionService;
    private readonly IGameSessionMessageService _gameSessionMessageService;
    private readonly ICharacterService _characterService;
    private readonly IGameSessionCharacterService _gameSessionCharacterService;
    private readonly IMapService _mapService;
    private readonly IGameSessionCharacterTurnInfoService _gameSessionCharacterTurnInfoService;
    private readonly ICharacterAbilityService _characterAbilityService;
    private readonly IGameSessionCharacterTurnQueueService _turnQueueService;

    private static readonly ConnectedUsers ConnectedUsers = new();
    private static readonly LockedCharacters LockedCharacters = new();
    private static readonly RequestedSaveThrows RequestedSaveThrows = new();

    public GameSessionsHub(
        IUserService userService,
        IGameSessionService gameSessionService,
        IGameSessionMessageService gameSessionMessageService,
        ICharacterService characterService,
        IGameSessionCharacterService gameSessionCharacterService,
        IMapService mapService,
        IGameSessionCharacterTurnInfoService gameSessionCharacterTurnInfoService,
        ICharacterAbilityService characterAbilityService,
        IGameSessionCharacterTurnQueueService turnQueueService)
    {
        _userService = userService;
        _gameSessionService = gameSessionService;
        _gameSessionMessageService = gameSessionMessageService;
        _characterService = characterService;
        _gameSessionCharacterService = gameSessionCharacterService;
        _mapService = mapService;
        _gameSessionCharacterTurnInfoService = gameSessionCharacterTurnInfoService;
        _characterAbilityService = characterAbilityService;
        _turnQueueService = turnQueueService;
    }

    /// <summary>
    /// Присоединится к игровой комнате
    /// </summary>
    /// <param name="gameSessionId"></param>
    /// <returns></returns>
    /// <exception cref="HubException"></exception>
    public async Task JoinGameSession(Guid gameSessionId)
    {
        var gameSession = await _gameSessionService.GetById(gameSessionId);

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

        await ConnectPlayer_Internal(gameSessionId, callerUser);
        await SendHistory(gameSession.Id, Clients.Caller);
        await SendPlayerInfos(gameSession, Clients.Group(gameSession.Id.ToString()));
        await SendGameSessionInfo(gameSession.Id, gameSession.GameSessionStatus, Clients.Caller);
    }

    /// <summary>
    /// Выйти из игровой комнаты
    /// </summary>
    /// <returns></returns>
    /// <exception cref="HubException"></exception>
    public async Task LeaveGameSession()
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

        if (!ConnectedUsers.IsConnected(gameSessionId, caller.Id))
        {
            throw new HubException("User not connected to this session");
        }

        await LeaveGameSession_Internal(gameSessionId, caller, Context.ConnectionId);
    }

    /// <summary>
    /// Отправка сообщения в чат
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task NewMessage(string message)
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
        await NewMessage_Internal(gameSessionId, message, caller.Id);
    }

    /// <summary>
    /// Кинуть кубик
    /// </summary>
    /// <param name="cubeTypeEnum"></param>
    /// <returns></returns>
    public async Task RollDice(CubeTypeEnum cubeTypeEnum)
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

        await RollDice_Internal(gameSessionId, cubeTypeEnum, caller);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await LeaveGameSession();
        await base.OnDisconnectedAsync(exception);
    }
}