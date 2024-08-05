using Magic.Domain.Entities;
using Magic.Domain.Enums;

namespace Magic.Service.Interfaces;

public interface IGameSessionMessageService
{
    Task<ChatGameGameSessionMessage> AddChatMessage(Guid gameSessionId, string message);
    Task<ServerGameSessionMessage> AddServerMessage(Guid gameSessionId, string message);
    Task<DiceGameSessionMessage> AddDiceMessage(Guid gameSessionId, int diceRoll, CubeTypeEnum cubeTypeEnum);
    Task<List<BaseGameSessionMessage>> GetMessages(Guid gameSessionId);
}