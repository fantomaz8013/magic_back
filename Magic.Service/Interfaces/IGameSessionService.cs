using Magic.Common.Models.Request.GameSessionRequest;
using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Magic.Domain.Enums;

namespace Magic.Service.Interfaces;

public interface IGameSessionService
{
    Task<GameSessionResponse> Create(CreateGameSessionRequest request);
    Task<bool> Enter(EnterToGameSessionRequest request);
    Task<bool> Kick(KickUserForGameSessionRequest request);
    Task<bool> Delete (DeleteGameSessionRequest request);
    Task<bool> Leave(LeaveGameSessionRequest request);
    Task<List<GameSessionResponse>> GetAllGameSession();
    Task<GameSession?> GetById(Guid gameSessionId);
    /// <summary>
    /// Изменить статус игровой сессии. Статус меняется только по увеличению.
    /// </summary>
    /// <param name="gameSessionId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<bool> ChangeGameSessionStatus(Guid gameSessionId, GameSessionStatusTypeEnum status);
}