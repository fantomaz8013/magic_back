namespace Magic.Domain.Entities;

public class Map : BaseEntity<Guid>
{
    public List<List<int>> Tiles { get; set; }
}