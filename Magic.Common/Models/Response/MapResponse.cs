using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class MapResponse
{
    public Guid Id { get; set; }
    public List<List<int>> Tiles { get; set; }

    public MapResponse(Map map)
    {
        Id = map.Id;
        Tiles = map.Tiles;
    }
}