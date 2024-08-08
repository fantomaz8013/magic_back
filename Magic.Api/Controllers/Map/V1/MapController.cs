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
    protected readonly ITilePropertyService _tilePropertyService;

    public MapController(IMapService mapService, ITilePropertyService tilePropertyService)
    {
        _mapService = mapService;
        _tilePropertyService = tilePropertyService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<MapResponse>>))]
    public async Task<IActionResult> List()
    {
        var result = await _mapService.GetMaps();
        return Ok(result);
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseData<List<TileProperty>>))]
    public async Task<IActionResult> TileProperties()
    {
        var result = await _tilePropertyService.GetTileProperties();
        return Ok(result);
    }
}