using Magic.Api.Controller.Base;
using Magic.Domain.Entities;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Controller.User.V1;

public class CharacterController : V1CharacterControllerBase
{
    protected readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<CharacterAvatar>>))]
    public async Task<IActionResult> DefaultAvatar()
    {
        var result = await _characterService.GetDefaultAvatar();
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<CharacterCharacteristic>>))]
    public async Task<IActionResult> Characteristics()
    {
        var result = await _characterService.GetCharacterCharacteristics();
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<CharacterClass>>))]
    public async Task<IActionResult> Classes()
    {
        var result = await _characterService.GetClasses();
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<CharacterRace>>))]
    public async Task<IActionResult> Races()
    {
        var result = await _characterService.GetCharacterRaces();
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<CharacterClass>>))]
    public async Task<IActionResult> Templates()
    {
        var result = await _characterService.GetCharacterTemplates();
        return Ok(result);
    }
}