using Magic.Domain.Entities;

namespace Magic.Service.Interfaces;

public interface IGameSessionCharacterTurnQueueService
{
    Task<GameSessionCharacterTurnQueue> InitTurnQueue(Guid gameSessionId);
    Task EndTurnQueue(Guid gameSessionId);
    Task<int> NextTurnQueue(Guid gameSessionId);
    Task<GameSessionCharacterTurnQueue?> GetTurnQueue(Guid gameSessionId);
}