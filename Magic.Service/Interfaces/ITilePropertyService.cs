using Magic.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Magic.Service.Interfaces;

public interface ITilePropertyService
{
    Task<List<TileProperty>> GetTileProperties();
    Task<TileProperty> GetTileProperty(int id);
}