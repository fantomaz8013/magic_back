using Magic.Api.Controller.Base;
using Magic.Common.Models.Request;
using Magic.Common.Models.Response;
using Magic.Service;
using Microsoft.AspNetCore.Mvc;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Controller.Token.V1;

public class TokenController : V1TokenControllerBase
{
    protected readonly IUserService _userService;
    public TokenController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<AuthResponse>))]
    public async Task<IActionResult> ByLogin([FromBody] TokenRequest token)
    {
        var tokenResponse = await _userService.Login(token);
        return Ok(tokenResponse);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<TokenResponse>))]
    public async Task<IActionResult> Refresh([FromBody] string token)
    {
        var tokenResponse = await _userService.RefreshToken(token);
        return Ok(tokenResponse);
    }
}