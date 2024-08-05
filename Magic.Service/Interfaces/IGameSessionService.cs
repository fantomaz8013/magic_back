using Magic.Common.Models.Request.GameSessionRequest;
using Magic.Common.Models.Response;

namespace Magic.Service.Interfaces;

public interface IGameSessionService
{
    Task<bool> Create(CreateGameSessionRequest request);
    Task<bool> Enter(EnterToGameSessionRequest request);
    Task<bool> Kick(KickUserForGameSessionRequest request);
    Task<bool> Delete (DeleteGameSessionRequest request);
    Task<bool> Leave(LeaveGameSessionRequest request);
    Task<List<GameSessionResponse>> GetAllGameSession();

}