using Magic.Api.Controllers.Websockets.Requests;
using Magic.Common.Models.Response;
using Magic.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets
{
    /// <summary>
    /// В этом файле описываются все механики для мастера игры ( они все должны внутри себя вызывать IsCallerAdmin() ) <- поменять на Authorize("master")
    /// </summary>
    partial class GameSessionsHub
    {
        /// <summary>
        /// Изменить персонажа
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="changedCharacterFields"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
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

        /// <summary>
        /// Выгнать из игровой комнаты
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
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

        /// <summary>
        /// Запустить игру ( мастер нажимает кнопкку старта на экране выбора персонажей )
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task StartGame()
        {
            var (gameSessionId, callerUser) = await GetConnectionInfoOrThrow();
            var gameSession = await _gameSessionService.GetById(gameSessionId);

            if (!IsCallerAdmin(gameSession, callerUser.Id))
                throw new HubException("You have no rights to run this action");

            var locks = LockedCharacters.GetGameSessionLocks(gameSessionId);
            if (locks is null)
                throw new HubException("There is no locked characters, game can not be started");


            await _characterService.AddCharactersToGameSession(
                locks.Select(l => (l.Key, l.Value)).ToList(),
                gameSessionId
            );
            LockedCharacters.UnlockByGameSession(gameSessionId);
            if (!await _gameSessionService.ChangeGameSessionStatus(gameSessionId, GameSessionStatusTypeEnum.InGame))
                throw new HubException("Couldn't start game");

            var group = Clients.Group(gameSessionId.ToString());
            await SendGameSessionInfo(gameSession.Id, GameSessionStatusTypeEnum.InGame, group);
        }

        /// <summary>
        /// Запросить спас бросок для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="characterCharacteristicId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task RequestSaveThrow(Guid userId, int characterCharacteristicId, int value)
        {
            var (gameSessionId, callerUser) = await GetConnectionInfoOrThrow();
            var gameSession = await _gameSessionService.GetById(gameSessionId);

            if (!IsCallerAdmin(gameSession, callerUser.Id))
                throw new HubException("You have no rights to run this action");

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

        /// <summary>
        /// Закончить пошаговый режим
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task EndTurnBased()
        {
            var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

            if (!await IsCallerAdmin(gameSessionId, caller))
                throw new HubException("You have no rights to run this action");

            await _turnQueueService.EndTurnQueue(gameSessionId);
            await _gameSessionService.SetMap(gameSessionId, null);
            await _characterService.DeleteNpc(gameSessionId);

            var gameSessionProxy = Clients.Group(gameSessionId.ToString());
            await gameSessionProxy.SendAsync(Events.TurnBasedEnd);
            //todo send clear map?
            await SendGameSessionInfo(gameSessionId, GameSessionStatusTypeEnum.InGame, gameSessionProxy);
        }

        /// <summary>
        /// Запустить режим по ходам
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task StartTurnBased(Guid mapId)
        {
            var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

            if (!await IsCallerAdmin(gameSessionId, caller))
                throw new HubException("You have no rights to run this action");


            var templates = await _characterService.GetCharacterTemplates();
            //todo add npcs from argument
            var npcs = new List<(Guid OwnerId, Guid CharacterTemplateId)>
            {
                (caller.Id, templates[2].Id),
                (caller.Id, templates[3].Id)
            };

            await _gameSessionService.SetMap(gameSessionId, mapId);
            await _characterService.AddCharactersToGameSession(
                npcs,
                gameSessionId
            );
            var characters = await _characterService.GetGameSessionCharacters(gameSessionId);
            var x = 0;
            var y = 0;
            foreach (var character in characters)
            {
                //todo set positions from argument
                await _gameSessionCharacterService.SetPosition(character.Id, ++x, ++y);
            }

            var turnQueue = await _turnQueueService.InitTurnQueue(gameSessionId);

            var gameSessionProxy = Clients.Group(gameSessionId.ToString());
            await SendGameSessionInfo(gameSessionId, GameSessionStatusTypeEnum.InGame, gameSessionProxy);
            await gameSessionProxy.SendAsync(Events.TurnBasedInit, new TurnQueueResponse(turnQueue));

            var newChar = characters.FirstOrDefault(c => c.Id == turnQueue.GameSessionCharacterIds[turnQueue.CurrentIndex]);
            var nextTurnConnectionId = ConnectedUsers.GetConnectionId(gameSessionId, newChar.OwnerId);
            var turnInfo = await _gameSessionCharacterTurnInfoService.GetCharacterTurnInfo(newChar.Id);
            await Clients.Client(nextTurnConnectionId).SendAsync(Events.YourTurnStart, new TurnInfoResponse(turnInfo));
        }
    }
}
