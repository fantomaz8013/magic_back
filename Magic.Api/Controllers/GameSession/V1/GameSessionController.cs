using Magic.Api.Controller.Base;
using Magic.Common.Models.Request.GameSessionRequest;
using Magic.Common.Models.Response;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Controller.User.V1;

public class GameSessionController : V1GameSessionControllerBase
{
    protected readonly IGameSessionService _gameSessionService;

    public GameSessionController(IGameSessionService gameSessionService)
    {
        _gameSessionService = gameSessionService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<GameSessionResponse>>))]
    public async Task<IActionResult> List()
    {
        var result = await _gameSessionService.GetAllGameSession();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> Create([FromBody] CreateGameSessionRequest request)
    {
        var result = await _gameSessionService.Create(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> SetMap([FromBody] SetMapRequest request)
    {
        var result = await _gameSessionService.SetMap(request.GameSessionId, request.MapId);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> Enter([FromBody] EnterToGameSessionRequest request)
    {
        var result = await _gameSessionService.Enter(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> Kick([FromForm] KickUserForGameSessionRequest request)
    {
        var result = await _gameSessionService.Kick(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> Leave([FromForm] LeaveGameSessionRequest request)
    {
        var result = await _gameSessionService.Leave(request);
        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> Delete([FromBody] DeleteGameSessionRequest request)
    {
        var result = await _gameSessionService.Delete(request);
        return Ok(result);
    }
}