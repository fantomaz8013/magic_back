using Magic.Common.Models.Response;

namespace Magic.Service.Interfaces;

public interface IMapService
{
    Task<List<MapResponse>> GetMaps();
}