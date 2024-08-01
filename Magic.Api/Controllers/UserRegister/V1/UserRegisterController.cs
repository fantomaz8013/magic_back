using Magic.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Magic.Service;
using System.Threading.Tasks;
using Magic.Api.Controller.Base;

namespace Magic.Api.Controller.UserRegister.V1
{
    public class UserRegisterController : V1UserRegisterControllerBase
    {
        protected readonly IUserService _userService;
        public UserRegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [ActionName("user/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRequest user)
        {
            var result = await _userService.Register(user);
            return Ok(result);
        }
    }
}
