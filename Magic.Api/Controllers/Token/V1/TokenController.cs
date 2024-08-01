using Magic.Api.Controller.Base;
using Magic.Common.Models.Request;
using Magic.Service;
using Microsoft.AspNetCore.Mvc;

namespace Magic.Api.Controller.Token.V1
{
    public class UserRegisterController : V1TokenControllerBase
    {
        protected readonly IUserService _userService;
        public UserRegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [ActionName("token")]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] TokenRequest token)
        {
            var tokenResponse = await _userService.Login(token);
            return Ok(tokenResponse);
        }

        [ActionName("token/refresh")]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] string token)
        {
            var tokenResponse = await _userService.RefreshToken(token);
            return Ok(tokenResponse);
        }
    }
}
