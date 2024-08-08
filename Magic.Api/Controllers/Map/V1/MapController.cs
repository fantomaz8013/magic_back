using Magic.Api.Controller.Base;
using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Controller.User.V1;

public class MapController : V1MapControllerBase
{
    protected readonly IMapService _mapService;

    public MapController(IMapService mapService)
    {
        _mapService = mapService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<MapResponse>>))]
    public async Task<IActionResult> List()
    {
        var result = await _mapService.GetMaps();
        return Ok(result);
    }
}