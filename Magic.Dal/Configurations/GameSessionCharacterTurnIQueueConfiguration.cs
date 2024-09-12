using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Magic.DAL.Configurations;

public class GameSessionCharacterTurnQueueConfiguration : IEntityTypeConfiguration<GameSessionCharacterTurnQueue>
{
    public void Configure(EntityTypeBuilder<GameSessionCharacterTurnQueue> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(GameSessionCharacterTurnQueue));
        builder.HasBaseEntityGuid();

        builder.PropertyWithUnderscore(x => x.CurrentIndex);

        builder.PropertyWithUnderscore(x => x.GameSessionId);
        builder.HasForeignKey(x => x.GameSession, x => x.GameSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.GameSession);

        var serializeOptions = new JsonSerializerOptions();
        builder.PropertyWithUnderscore(x => x.GameSessionCharacterIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, serializeOptions),
                v => JsonSerializer.Deserialize<List<Guid>>(v, serializeOptions)
            );
    }
}