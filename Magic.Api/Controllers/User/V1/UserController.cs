using Magic.Api.Controller.Base;
using Magic.DAL.Extensions;
using Magic.Service;
using Microsoft.AspNetCore.Mvc;

namespace Magic.Api.Controller.User.V1
{
    public class UserController : V1UserControllerBase
    {
        protected readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [ActionName("user")]
        [HttpGet]
        public async Task<IActionResult> CurrentUser()
        {
            var result = await _userService.CurrentUser();
            return Ok(result);
        }

        [ActionName("users")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PagedRequest request)
        {
            var payments = await _userService.ListAsync(request);
            return Ok(payments);
        }
    }
}
