using Magic.Common.Models.Response;
using Magic.Domain.Enums;

namespace Magic.Service.Interfaces;

public interface IGameSessionMessageService
{
    Task<ChatGameGameSessionMessageResponse> AddChatMessage(Guid gameSessionId, string message, Guid userId);
    Task<ServerGameSessionMessageResponse> AddServerMessage(Guid gameSessionId, string message);

    Task<DiceGameSessionMessageResponse> AddDiceMessage(Guid gameSessionId, int diceRoll, CubeTypeEnum cubeTypeEnum,
        Guid userId);

    Task<List<BaseGameSessionMessageResponse>> GetMessages(Guid gameSessionId);
}