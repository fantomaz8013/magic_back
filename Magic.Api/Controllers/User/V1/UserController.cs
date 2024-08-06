using Magic.Api.Controller.Base;
using Magic.Common.Models.Request;
using Magic.Common.Models.Response;
using Magic.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Controller.User.V1;

public class UserController : V1UserControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<UserResponse>))]
    public async Task<IActionResult> CurrentUser()
    {
        var result = await _userService.CurrentUser();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<bool>))]
    public async Task<IActionResult> Update([FromForm] UserUpdateRequest request)
    {
        var result = await _userService.UpdateUser(request);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<TokenResponse>))]
    public async Task<IActionResult> Register([FromBody] UserRequest user)
    {
        var result = await _userService.Register(user);
        return Ok(result);
    }
}