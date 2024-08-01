using Magic.Api.Controller.Base;
using Magic.Common.Models.Request;
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

        [ActionName("user")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UserUpdateRequest request)
        {
            var result = await _userService.UpdateUser(request);
            return Ok(result);
        }
    }
}
