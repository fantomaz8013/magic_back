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

[Authorize]
public class GameSessionsHub : Hub
{
    private readonly IUserService _userService;
    private readonly IGameSessionService _gameSessionService;
    private readonly IGameSessionMessageService _gameSessionMessageService;
    private readonly ICharacterService _characterService;
    private readonly IGameSessionCharacterService _gameSessionCharacterService;

    private static readonly ConnectedUsers ConnectedUsers = new();
    private static readonly LockedCharacters LockedCharacters = new();
    private static readonly RequestedSaveThrows RequestedSaveThrows = new();

    public GameSessionsHub(
        IUserService userService,
        IGameSessionService gameSessionService,
        IGameSessionMessageService gameSessionMessageService,
        ICharacterService characterService,
        IGameSessionCharacterService gameSessionCharacterService
    )
    {
        _userService = userService;
        _gameSessionService = gameSessionService;
        _gameSessionMessageService = gameSessionMessageService;
        _characterService = characterService;
        _gameSessionCharacterService = gameSessionCharacterService;
    }

    private async Task<(Guid GameSessionId, UserResponse user)> GetConnectionInfoOrThrow()
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is null)
            throw new HubException("GameSession not found");

        var caller = await _userService.CurrentUser();

        return (gameSessionId.Value, caller!);
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

        await RollDice_Internal(gameSessionId, cubeTypeEnum, caller);
    }

    private async Task RollDice_Internal(Guid gameSessionId, CubeTypeEnum cubeTypeEnum, UserResponse caller)
    {
        var rollDice = DiceUtil.RollDice(cubeTypeEnum);
        var messageEntity =
            await _gameSessionMessageService.AddDiceMessage(gameSessionId, rollDice, cubeTypeEnum, caller.Id);
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

    private async Task SendPlayerInfos(Domain.Entities.GameSession gameSession, IClientProxy clientProxy)
    {
        var locks = LockedCharacters.GetGameSessionLocks(gameSession.Id);
        var playerInfos = gameSession
            .Users
            .Append(gameSession.CreatorUser)
            .Select(u =>
                {
                    var connectionId = ConnectedUsers.GetConnectionId(gameSession.Id, u.Id);
                    Guid? lockedCharacterId = null;
                    if (locks != null && locks.TryGetValue(u.Id, out var value))
                        lockedCharacterId = value;
                    return new PlayerInfo(
                        u.Id,
                        u.Login,
                        gameSession.CreatorUserId == u.Id,
                        lockedCharacterId,
                        connectionId != null
                    );
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
            var gameSession = await _gameSessionService.GetById(gameSessionId);
            gameSessionInfo = gameSessionInfo with
            {
                Characters = await _characterService.GetGameSessionCharacters(gameSessionId),
                Map = gameSession!.Map != null ? new MapResponse(gameSession.Map) : null,
            };
        }

        await clientProxy.SendAsync(Events.GameSessionInfoReceived, gameSessionInfo);
    }

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

        await LeaveGameSession_Internal(gameSessionId, caller, Context.ConnectionId);
    }

    private async Task LeaveGameSession_Internal(Guid gameSessionId, UserResponse leavingUser, string connectionId)
    {
        ConnectedUsers.Disconnect(gameSessionId, leavingUser.Id);
        LockedCharacters.UnlockByUser(gameSessionId, leavingUser.Id);
        await CreateServerMessage_Internal(gameSessionId, $"Player \"{leavingUser.Login}\" disconnected");
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.PlayerLeft, leavingUser.Id);
        await Groups.RemoveFromGroupAsync(connectionId, gameSessionId.ToString());
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

    public async Task Kick(Guid userId)
    {
        var (gameSessionId, callerUser) = await GetConnectionInfoOrThrow();
        var gameSession = await _gameSessionService.GetById(gameSessionId);

        if (!IsCallerAdmin(gameSession, callerUser.Id))
            throw new HubException("You have no rights to run this action");

        var userToKick = gameSession.Users.FirstOrDefault(u => u.Id == userId);
        var connectionId = ConnectedUsers.GetConnectionId(gameSessionId, userToKick.Id);
        if (userToKick is not null)
            await LeaveGameSession_Internal(gameSessionId, new UserResponse(userToKick), connectionId);
    }

    public async Task RequestSaveThrow(Guid userId, int characterCharacteristicId, int value)
    {
        var (gameSessionId, callerUser) = await GetConnectionInfoOrThrow();
        var gameSession = await _gameSessionService.GetById(gameSessionId);

        if (gameSession == null || gameSession.CreatorUserId != callerUser.Id)
        {
            throw new HubException("You have no rights to run this action");
        }

        var connectionId = ConnectedUsers.GetConnectionId(gameSessionId, userId);

        if (connectionId is null)
        {
            throw new HubException("User not connected");
        }

        var userToRequest = gameSession.Users.FirstOrDefault(u => u.Id == userId);
        var requestedSaveThrow = new RequestedSaveThrow(characterCharacteristicId, value, callerUser.Id, userId);
        if (userToRequest is not null)
            await RequestSaveThrow_Internal(connectionId, requestedSaveThrow);
    }

    private async Task RequestSaveThrow_Internal(
        string connectionId,
        RequestedSaveThrow requestedSaveThrow
    )
    {
        RequestedSaveThrows.RequestSaveThrow(connectionId, requestedSaveThrow);
        await Clients
            .Client(connectionId)
            .SendAsync(Events.PlayerSaveThrow, requestedSaveThrow);
    }

    public async Task RollSaveDice()
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
        var requestSaveThrow = RequestedSaveThrows.PassSaveThrow(Context.ConnectionId);
        if (requestSaveThrow is null)
        {
            throw new HubException("No save throws requested");
        }

        await RollSaveDice_Internal(gameSessionId, caller, requestSaveThrow);
    }

    private async Task RollSaveDice_Internal(
        Guid gameSessionId,
        UserResponse caller,
        RequestedSaveThrow requestSaveThrow)
    {
        var characteristics = await _characterService.GetCharacterCharacteristics();
        var characteristic = characteristics.FirstOrDefault(c => c.Id == requestSaveThrow.CharacterCharacteristicId);
        var modificator =
            await GetModificator(gameSessionId, requestSaveThrow.UserId, requestSaveThrow.CharacterCharacteristicId);

        var rollDice = DiceUtil.RollDice(CubeTypeEnum.D20);
        var resultRollDice = rollDice + modificator;

        var messageEntity =
            await _gameSessionMessageService.AddDiceMessage(gameSessionId, resultRollDice, CubeTypeEnum.D20, caller.Id);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.MessageReceived, messageEntity);
        var message =
            BuildSaveThrowMessage(requestSaveThrow, rollDice, caller.Login, characteristic!.Title, modificator);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.PlayerSaveThrowPassed,
            new RequestedSaveThrowPassed(requestSaveThrow, resultRollDice));
        await CreateServerMessage_Internal(gameSessionId, message);
    }

    private async Task<int> GetModificator(Guid gameSessionId, Guid userId, int characterCharacteristicId)
    {
        var gameSessionCharacters = await _characterService.GetGameSessionCharacters(gameSessionId);
        var gameSessionCharacter = gameSessionCharacters.FirstOrDefault(c => c.OwnerId == userId)!;
        return await _gameSessionCharacterService.GetRollModificator(gameSessionCharacter.Id,
            characterCharacteristicId);
    }

    private string BuildSaveThrowMessage(
        RequestedSaveThrow requestSaveThrow,
        int rollDice,
        string callerLogin,
        string characteristicTitle,
        int modificator)
    {
        var hasPassed = rollDice + modificator >= requestSaveThrow.Value;
        var passedMessage = hasPassed ? "прошёл" : "провалил";
        return
            @$"Игрок {callerLogin} {passedMessage} проверку на {characteristicTitle} сложностью в {requestSaveThrow.Value}, выбросив {rollDice} с модификатором {modificator} (={rollDice + modificator}).";
    }

    public async Task ChangeCharacter(Guid characterId, Dictionary<string, string> changedCharacterFields)
    {
        var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
        if (!await IsCallerAdmin(gameSessionId, caller))
            throw new HubException("You have no rights to run this action");

        var gameSessionCharacters = await _characterService.GetGameSessionCharacters(gameSessionId);
        var gameSessionCharacter = gameSessionCharacters.FirstOrDefault(c => c.Id == characterId);
        await _gameSessionCharacterService.Change(gameSessionCharacter, changedCharacterFields);
        await SendGameSessionInfo(
            gameSessionId,
            GameSessionStatusTypeEnum.InGame,
            Clients.Group(gameSessionId.ToString())
        );
    }

    private async Task<bool> IsCallerAdmin(Guid gameSessionId, UserResponse caller)
    {
        var gameSession = await _gameSessionService.GetById(gameSessionId);

        return IsCallerAdmin(gameSession, caller.Id);
    }

    private bool IsCallerAdmin(Domain.Entities.GameSession? gameSession, Guid callerId)
    {
        return gameSession != null && gameSession.CreatorUserId == callerId;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await LeaveGameSession();

        await base.OnDisconnectedAsync(exception);
    }
}