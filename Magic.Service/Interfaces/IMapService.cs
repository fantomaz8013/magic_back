using Magic.Common.Models.Request;
using Magic.Common.Models.Response;

namespace Magic.Service.Interfaces;

public interface IMapService
{
    Task<List<MapResponse>> GetMaps();
    /// <summary>
    /// Расчитать, можно ли пройти по указаному маршруту в карте. path = (индекс пути, LocationRequest) пример: [<0,Location(2,3)>, <1,Location(3,4)>, <2,Location(3,5)>]
    /// </summary>
    /// <param name="path"> List<LocationRequest> пример: [Location(2,3), Location(3,4), Location(3,5)]</param>
    /// <param name="mapId">Идентификатор карты</param>
    /// <param name="gameSessionCharacterId">Идентификатор игрового персонажа</param>
    /// <returns></returns>
    Task<PathCalculationResponse> PathCalculation(List<LocationRequest> path, Guid mapId, Guid gameSessionCharacterId);
}