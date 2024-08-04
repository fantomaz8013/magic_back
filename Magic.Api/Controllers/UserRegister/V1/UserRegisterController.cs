using Magic.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Magic.Service;
using Magic.Api.Controller.Base;
using Magic.Common.Models.Response;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Controller.UserRegister.V1;

public class UserRegisterController : V1UserRegisterControllerBase
{
    protected readonly IUserService _userService;
    public UserRegisterController(IUserService userService)
    {
        _userService = userService;
    }

    [ActionName("user/register")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<TokenResponse>))]
    public async Task<IActionResult> Register([FromBody] UserRequest user)
    {
        var result = await _userService.Register(user);
        return Ok(result);
    }
}