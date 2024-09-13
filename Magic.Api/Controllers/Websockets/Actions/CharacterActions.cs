using Magic.Common.Models.Request;
using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets
{
    /// <summary>
    /// В этом файле описываются все механики связанные с персонажами
    /// </summary>
    partial class GameSessionsHub
    {
        /// <summary>
        /// Закончить ход
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public async Task EndTurn(Guid characterId)
        {
            var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
            var character = await _gameSessionCharacterService.GetGameSessionCharacter(characterId);
            var turnQueue = await _turnQueueService.GetTurnQueue(gameSessionId);
            if (character.OwnerId != caller.Id || turnQueue is null)
            {
                //todo послать нахуй
                return;
            }

            var newIndex = await _turnQueueService.NextTurnQueue(gameSessionId);
            var gameSessionCharacters = await _characterService.GetGameSessionCharacters(gameSessionId);
            var newChar = gameSessionCharacters.FirstOrDefault(c => c.Id == turnQueue.GameSessionCharacterIds[newIndex]);
            var nextTurnConnectionId = ConnectedUsers.GetConnectionId(gameSessionId, newChar.OwnerId);
            //todo map to response
            var newTurnInfo = await _gameSessionCharacterTurnInfoService.UpdateTurnInfo(character.Id);

            await Clients.Group(gameSessionId.ToString()).SendAsync(Events.NextTurn, newIndex);
            //todo send after move, ability and so on
            await Clients.Client(nextTurnConnectionId).SendAsync(Events.YourTurnStart, new TurnInfoResponse(newTurnInfo));
        }

        /// <summary>
        /// Передвижение персонажа
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task MoveCharacter(Guid characterId, List<LocationRequest> path)
        {
            var (gameSessionId, caller) = await GetConnectionInfoOrThrow();
            var character = await _gameSessionCharacterService.GetGameSessionCharacter(characterId);
            var gameSession = await _gameSessionService.GetById(gameSessionId);
            if (caller.Id != character.OwnerId)
            {
                throw new HubException("You have no rights to run this command");
            }

            if (!gameSession.MapId.HasValue)
            {
                throw new HubException("There is no map");
            }

            var pathCalc = await _mapService.PathCalculation(path, gameSession.MapId.Value, characterId);
            if (pathCalc.IsSuccess)
            {
                await _gameSessionCharacterService.SetPosition
                (characterId,
                    pathCalc.NewCharacterPosition.X,
                    pathCalc.NewCharacterPosition.Y
                );
            }
            else
            {
                throw new HubException("Incorrect path");
            }

            if (pathCalc.Penalties.Count > 0)
            {
                await _gameSessionCharacterService.Damage(characterId, pathCalc.Penalties
                    .Where(p => p.PenaltyType == TilePropertyPenaltyTypeEnum.PenaltyHealth)
                    .Sum(p => p.Value));
            }

            await SendGameSessionInfo(
                gameSessionId,
                GameSessionStatusTypeEnum.InGame,
                Clients.Group(gameSessionId.ToString())
            );
        }

        /// <summary>
        /// Использовать способность
        /// </summary>
        /// <param name="characterAbilityId"></param>
        /// <param name="casterGameSessionCharacterId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public async Task UseAbility(int characterAbilityId, Guid casterGameSessionCharacterId, int x, int y)
        {
            var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

            var applyAbility =
                await _characterAbilityService.ApplyAbility(characterAbilityId, casterGameSessionCharacterId, x, y);

            foreach (var message in applyAbility.Messages)
                if (applyAbility.IsPossible)
                {
                    await CreateServerMessage_Internal(gameSessionId, message);
                }
                else
                {
                    await Clients.Caller.SendAsync(Events.MessageReceived,
                        new ServerGameSessionMessageResponse(
                            new ServerGameSessionMessage
                            {
                                GameSessionId = gameSessionId,
                                Message = message,
                                CreatedDate = DateTime.UtcNow
                            })
                    );
                }

            if (applyAbility.IsPossible)
                await SendGameSessionInfo(gameSessionId, GameSessionStatusTypeEnum.InGame,
                    Clients.Group(gameSessionId.ToString()));
        }

        /// <summary>
        /// Выбор персонажа ( игрок блокирует персонаж за собой ) 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
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

        /// <summary>
        /// Освободить текущего выбраного персонажа
        /// </summary>
        /// <returns></returns>
        public async Task UnlockCharacter()
        {
            var (gameSessionId, caller) = await GetConnectionInfoOrThrow();

            await UnlockCharacter_Internal(gameSessionId, caller.Id);
        }

        /// <summary>
        /// Выполнить запрошенный спас бросок от мастера
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
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
    }
}
