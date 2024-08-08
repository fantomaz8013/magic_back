using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using System.Security.Claims;

namespace Magic.Service.Interfaces;

public interface IMapService
{
    Task<List<MapResponse>> GetMaps();
}