using Magic.Common.Models.Response;
using Magic.Domain.Enums;

namespace Magic.Service.Interfaces;

public interface IGameSessionMessageService
{
    Task<ChatGameGameSessionMessageResponse> AddChatMessage(Guid gameSessionId, string message);
    Task<ServerGameSessionMessageResponse> AddServerMessage(Guid gameSessionId, string message);
    Task<DiceGameSessionMessageResponse> AddDiceMessage(Guid gameSessionId, int diceRoll, CubeTypeEnum cubeTypeEnum);
    Task<List<BaseGameSessionMessageResponse>> GetMessages(Guid gameSessionId);
}