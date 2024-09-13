using Magic.Api.Controllers.Websockets.InMemory;
using Magic.Api.Controllers.Websockets.Requests;
using Magic.Api.Controllers.Websockets.Responses;
using Magic.Common.Models.Response;
using Magic.Domain.Enums;
using Magic.Utils;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets;

/// <summary>
/// В этом файле описаны все внутренние методы, которые используются в публичных
/// </summary>
partial class GameSessionsHub
{
    private async Task LockCharacter_Internal(Guid gameSessionId, Guid callerUserId, Guid characterId)
    {
        LockedCharacters.Lock(gameSessionId, callerUserId, characterId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.CharacterLocked, callerUserId, characterId);
    }

    private async Task UnlockCharacter_Internal(Guid gameSessionId, Guid callerUserId)
    {
        LockedCharacters.UnlockByUser(gameSessionId, callerUserId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.CharacterUnlocked, callerUserId);
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

    private async Task LeaveGameSession_Internal(Guid gameSessionId, UserResponse leavingUser, string connectionId)
    {
        ConnectedUsers.Disconnect(gameSessionId, leavingUser.Id);
        LockedCharacters.UnlockByUser(gameSessionId, leavingUser.Id);
        await CreateServerMessage_Internal(gameSessionId, $"Player \"{leavingUser.Login}\" disconnected");
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.PlayerLeft, leavingUser.Id);
        await Groups.RemoveFromGroupAsync(connectionId, gameSessionId.ToString());
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

    private async Task<int> GetModificator(Guid gameSessionId, Guid userId, int characterCharacteristicId)
    {
        var gameSessionCharacters = await _characterService.GetGameSessionCharacters(gameSessionId);
        var gameSessionCharacter = gameSessionCharacters.FirstOrDefault(c => c.OwnerId == userId)!;
        return await _gameSessionCharacterService.GetRollModificator(gameSessionCharacter.Id,
            characterCharacteristicId);
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

    private async Task<(Guid GameSessionId, UserResponse user)> GetConnectionInfoOrThrow()
    {
        var gameSessionId = ConnectedUsers.GetGameSessionId(Context.ConnectionId);

        if (gameSessionId is null)
            throw new HubException("GameSession not found");

        var caller = await _userService.CurrentUser();

        return (gameSessionId.Value, caller!);
    }

    private async Task NewMessage_Internal(Guid gameSessionId, string message, Guid userId)
    {
        var messageEntity = await _gameSessionMessageService.AddChatMessage(gameSessionId, message, userId);
        await Clients.Group(gameSessionId.ToString()).SendAsync(Events.MessageReceived, messageEntity);
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
}