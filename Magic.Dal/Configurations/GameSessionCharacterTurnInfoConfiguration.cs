using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Magic.DAL.Configurations;

public class GameSessionCharacterTurnInfoConfiguration : IEntityTypeConfiguration<GameSessionCharacterTurnInfo>
{
    public void Configure(EntityTypeBuilder<GameSessionCharacterTurnInfo> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(GameSessionCharacterTurnInfo));
        builder.HasBaseEntityInt();

        builder.PropertyWithUnderscore(x => x.LeftStep);
        builder.PropertyWithUnderscore(x => x.LeftMainAction);
        builder.PropertyWithUnderscore(x => x.LeftBonusAction);
        builder.PropertyWithUnderscore(x => x.SkipStepCount)
            .HasDefaultValue(0);

        builder.PropertyWithUnderscore(x => x.GameSessionCharacterId);
        builder.HasForeignKey(x => x.GameSessionCharacter, x => x.GameSessionCharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.GameSessionCharacter);

        var serializeOptions = new JsonSerializerOptions();
        builder.PropertyWithUnderscore(x => x.AbilityCoolDowns).HasConversion(
                v => JsonSerializer.Serialize(v, serializeOptions),
                v => JsonSerializer.Deserialize<List<AbilityCoolDowns>>(v, serializeOptions)
            );

        builder.PropertyWithUnderscore(x => x.BuffCoolDowns).HasConversion(
                v => JsonSerializer.Serialize(v, serializeOptions),
                v => JsonSerializer.Deserialize<List<BuffCoolDowns>>(v, serializeOptions)
            );
    }
}