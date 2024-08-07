using Magic.Common.Models.Response;
using Magic.Domain.Entities;
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
    private readonly ICharacterService _characterService;

    private static readonly ConnectedUsers ConnectedUsers = new();
    private static readonly LockedCharacters LockedCharacters = new();

    public GameSessionsHub(IUserService userService, IGameSessionService gameSessionService,
        IGameSessionMessageService gameSessionMessageService, ICharacterService characterService)
    {
        _userService = userService;
        _gameSessionService = gameSessionService;
        _gameSessionMessageService = gameSessionMessageService;
        _characterService = characterService;
    }

    private async Task<(Guid GameSessionId, UserResponse user)> GetConnectionInfoOrThrow()
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is null)
            throw new HubException("GameSession not found");
        var caller = await _userService.CurrentUser();

        if (caller is null)
            throw new HubException("401, Unauthorized");

        return (gameSessionId.Value, caller);
    }

    public async Task NewMessage(string message)
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
        await NewMessage_Internal(gameSessionId, message, caller.Id);
    }

    private async Task NewMessage_Internal(Guid gameSessionId, string message, Guid userId)
    {
        var messageEntity = await _gameSessionMessageService.AddChatMessage(gameSessionId, message, userId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.MessageReceived, messageEntity);
    }

    public async Task RollDice(CubeTypeEnum cubeTypeEnum)
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

        await RollDice_Internal(gameSessionId, cubeTypeEnum, caller.Id);
    }

    private async Task RollDice_Internal(Guid gameSessionId, CubeTypeEnum cubeTypeEnum, Guid callerId)
    {
        var rollDice = DiceUtil.RollDice(cubeTypeEnum);
        var messageEntity =
            await _gameSessionMessageService.AddDiceMessage(gameSessionId, rollDice, cubeTypeEnum, callerId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.MessageReceived, messageEntity);
    }

    private async Task CreateServerMessage_Internal(Guid gameSessionId, string message)
    {
        var messageEntity = await _gameSessionMessageService.AddServerMessage(gameSessionId, message);

        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.MessageReceived, messageEntity);
    }

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

    private async Task ConnectPlayer_Internal(Guid gameSessionId, UserResponse callerUser)
    {
        ConnectedUsers.Connect(gameSessionId, callerUser.Id, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, gameSessionId.ToString());
        await CreateServerMessage_Internal(gameSessionId, $"Player \"{callerUser.Login}\" joined!");
    }

    private async Task SendHistory(Guid gameSessionId, IClientProxy clientProxy)
    {
        var messages = await _gameSessionMessageService.GetMessages(gameSessionId);

        if (messages.Count > 0)
            await clientProxy.SendAsync(Events.HistoryReceived, messages);
    }

    private async Task SendPlayerInfos(GameSession gameSession, IClientProxy clientProxy)
    {
        var locks = LockedCharacters.GetGameSessionLocks(gameSession.Id);
        var playerInfos = gameSession
            .Users
            .Append(gameSession.CreatorUser)
            .Select(u =>
                {
                    Guid? lockedCharacterId = null;
                    if (locks != null && locks.TryGetValue(u.Id, out var value))
                        lockedCharacterId = value;
                    return new PlayerInfo(u.Id, u.Login, gameSession.CreatorUserId == u.Id, lockedCharacterId);
                }
            )
            .ToArray();
        await clientProxy.SendAsync(Events.PlayerInfoReceived, playerInfos);
    }

    private async Task SendGameSessionInfo(
        Guid gameSessionId,
        GameSessionStatusTypeEnum gameSessionStatus,
        IClientProxy clientProxy)
    {
        var gameSessionInfo = new GameSessionInfo(gameSessionStatus);
        if (gameSessionStatus == GameSessionStatusTypeEnum.InGame)
        {
            gameSessionInfo = gameSessionInfo with
            {
                Characters = await _characterService.GetGameSessionCharacters(gameSessionId)
            };
        }

        await clientProxy.SendAsync(Events.GameSessionInfoReceived, gameSessionInfo);
    }

    public record GameSessionInfo(
        GameSessionStatusTypeEnum GameSessionStatus,
        List<GameSessionCharacter>? Characters = null
    );

    public async Task LockCharacter(Guid characterId)
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
        var gameSession = await _gameSessionService.GetById(gameSessionId);
        if (gameSession.CreatorUserId == caller.Id)
            throw new HubException("GameMaster can't lock characters");

        if (LockedCharacters.IsCharacterLocked(gameSessionId, characterId))
            throw new HubException("Character already locked");

        await LockCharacter_Internal(gameSessionId, caller.Id, characterId);
    }

    private async Task LockCharacter_Internal(Guid gameSessionId, Guid callerUserId, Guid characterId)
    {
        LockedCharacters.Lock(gameSessionId, callerUserId, characterId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.CharacterLocked, callerUserId, characterId);
    }

    public async Task UnlockCharacter()
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

        await UnlockCharacter_Internal(gameSessionId, caller.Id);
    }

    private async Task UnlockCharacter_Internal(Guid gameSessionId, Guid callerUserId)
    {
        LockedCharacters.UnlockByUser(gameSessionId, callerUserId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.CharacterUnlocked, callerUserId);
    }

    public async Task LeaveGameSession()
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

        if (!ConnectedUsers.IsConnected(gameSessionId, caller.Id))
        {
            throw new HubException("User not connected to this session");
        }

        await LeaveGameSession_Internal(gameSessionId, caller);
    }

    private async Task LeaveGameSession_Internal(Guid gameSessionId, UserResponse callerUser)
    {
        await CreateServerMessage_Internal(gameSessionId, $"Player \"{callerUser.Login}\" disconnected");
        ConnectedUsers.Disconnect(gameSessionId, callerUser.Id);
        LockedCharacters.UnlockByUser(gameSessionId, callerUser.Id);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameSessionId.ToString());
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.PlayerLeft, callerUser.Id);
    }

    public async Task StartGame()
    {
        var (gameSessionId, callerUser) = await GetConnectionInfoOrThrow();
        var gameSession = await _gameSessionService.GetById(gameSessionId);

        if (gameSession == null || gameSession.CreatorUserId != callerUser.Id)
        {
            throw new HubException("You have no rights to run this action");
        }

        var locks = LockedCharacters.GetGameSessionLocks(gameSessionId);
        if (locks is null)
            throw new HubException("There is no locked characters, game can not be started");

        var characters =
            await _characterService.ChooseCharacters(
                locks,
                gameSessionId
            );
        LockedCharacters.UnlockByGameSession(gameSessionId);
        if (!await _gameSessionService.ChangeGameSessionStatus(gameSessionId, GameSessionStatusTypeEnum.InGame))
            throw new HubException("Couldn't start game");

        var group = Clients.Group(gameSessionId.ToString());
        await SendGameSessionInfo(gameSession.Id, GameSessionStatusTypeEnum.InGame, group);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await LeaveGameSession();

        await base.OnDisconnectedAsync(exception);
    }
}