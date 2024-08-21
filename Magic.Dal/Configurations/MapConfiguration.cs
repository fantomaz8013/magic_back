using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Magic.DAL.Configurations;

public class MapConfiguration : IEntityTypeConfiguration<Map>
{
    public void Configure(EntityTypeBuilder<Map> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(Map));
        builder.HasBaseEntityGuid();
        var serializeOptions = new JsonSerializerOptions();
        builder.PropertyWithUnderscore(x => x.Tiles).HasConversion(
                v => JsonSerializer.Serialize(v, serializeOptions),
                v => JsonSerializer.Deserialize<List<List<int>>>(v, serializeOptions)
            );

        builder.HasData(new Map
        {
            Id = Guid.Parse("1850beb4-ed84-4c7f-1234-cd7bce35e5d4"),
            Tiles = new List<List<int>> {
                new List<int> {5, 5, 5, 5, 5, 5, 5, 1, 2, 5, 1, 5, 5, 5, 1},
                new List<int> {5, 5, 5, 5, 5, 4, 5, 1, 5, 5, 1, 5, 5, 5, 1},
                new List<int> {5, 5, 5, 5, 5, 5, 5, 1, 5, 5, 1, 5, 5, 2, 1},
                new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                new List<int> {5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 1, 5, 3, 3, 1},
                new List<int> {5, 5, 5, 5, 5, 4, 4, 4, 4, 2, 1, 5, 3, 3, 1},
                new List<int> {1, 1, 3, 3, 3, 1, 1, 4, 4, 4, 1, 1, 1, 1, 1},
                new List<int> {5, 5, 3, 3, 3, 5, 5, 1, 5, 5, 1, 5, 5, 5, 1},
                new List<int> {5, 5, 3, 3, 3, 5, 5, 1, 5, 5, 1, 5, 5, 5, 1},
                new List<int> {5, 5, 5, 5, 5, 5, 5, 1, 5, 5, 1, 5, 5, 5, 2},
                new List<int> {5, 5, 5, 5, 5, 5, 5, 1, 5, 5, 2, 5, 5, 5, 1},
                new List<int> {2, 5, 5, 5, 5, 5, 5, 1, 5, 5, 1, 2, 2, 5, 1},
                new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                new List<int> {5, 5, 5, 5, 5, 5, 5, 1, 5, 5, 1, 5, 5, 5, 2},
                new List<int> {5, 5, 5, 5, 5, 5, 5, 1, 5, 5, 1, 5, 5, 2, 1},
            }
        });
    }
}