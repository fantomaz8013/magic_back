using System.Text.Json;
using System.Text.Json.Serialization;
using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class GameSessionCharacterConfiguration : IEntityTypeConfiguration<GameSessionCharacter>
{
    public void Configure(EntityTypeBuilder<GameSessionCharacter> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(GameSessionCharacter));
        builder.HasBaseEntityGuid();

        builder.PropertyWithUnderscore(x => x.Name);
        builder.PropertyWithUnderscore(x => x.Description);
        builder.PropertyWithUnderscore(x => x.AvatarUrL);

        builder.PropertyWithUnderscore(x => x.CharacterClassId);
        builder.HasForeignKey(x => x.CharacterClass, x => x.CharacterClassId);
        builder.HasOne(x => x.CharacterClass);

        builder.PropertyWithUnderscore(x => x.AbilitieIds);
        builder.PropertyWithUnderscore(x => x.Armor);

        builder.PropertyWithUnderscore(x => x.CharacterRaceId);
        builder.HasForeignKey(x => x.CharacterRace, x => x.CharacterRaceId);
        builder.HasOne(x => x.CharacterRace);

        builder.PropertyWithUnderscore(x => x.MaxHP);
        builder.PropertyWithUnderscore(x => x.Speed);
        builder.PropertyWithUnderscore(x => x.Initiative);

        builder.PropertyWithUnderscore(x => x.CurrentHP);
        builder.PropertyWithUnderscore(x => x.CurrentShield);
        builder.PropertyWithUnderscore(x => x.PositionX);
        builder.PropertyWithUnderscore(x => x.PositionY);

        builder.PropertyWithUnderscore(x => x.OwnerId);
        builder.HasForeignKey(x => x.Owner, x => x.OwnerId);
        builder.HasOne(x => x.Owner)
            .WithMany(x => x.GameSessionsCharacters);

        builder.PropertyWithUnderscore(x => x.GameSessionId);
        builder.HasForeignKey(x => x.GameSession, x => x.GameSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.GameSession);


        var serializeOptions = new JsonSerializerOptions();
        builder.PropertyWithUnderscore(x => x.Characteristics)
            .HasConversion(
                v => JsonSerializer.Serialize(v, serializeOptions),
                v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, serializeOptions)
            );
    }
}