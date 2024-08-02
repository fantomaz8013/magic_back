using Magic.Api.Controller.Base;
using Magic.Common.Models.Request;
using Magic.DAL.Extensions;
using Magic.Service;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Magic.Api.Controller.User.V1
{
    public class CharacterController : V1CharacterControllerBase
    {
        protected readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [ActionName("defaultavatar")]
        [HttpGet]
        public async Task<IActionResult> avatars()
        {
            var result = await _characterService.GetDefaultAvatar();
            return Ok(result);
        }
        [ActionName("characteristics")]
        [HttpGet]
        public async Task<IActionResult> Characteristics()
        {
            var result = await _characterService.GetCharacterCharacteristics();
            return Ok(result);
        }
        [ActionName("classes")]
        [HttpGet]
        public async Task<IActionResult> Classes()
        {
            var result = await _characterService.GetClasses();
            return Ok(result);
        }
    }
}
