using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(GameSession));
        builder.HasBaseEntityGuid();
        builder.HasCreatedDateEntity();

        builder.PropertyWithUnderscore(x => x.Title);
        builder.PropertyWithUnderscore(x => x.Description);
        builder.PropertyWithUnderscore(x => x.MaxUserCount);
        builder.PropertyWithUnderscore(x => x.CreatedDate).HasDateTimeConversion()
            .HasColumnType(SqlColumnTypes.TimeStampWithTimeZone); ;

        builder.HasMany(e => e.Users)
            .WithMany(e => e.GameSessions)
            .UsingEntity<GameSessionUser>(
            l => l.HasOne<User>().WithMany().HasForeignKey(e => e.UserId),
            r => r.HasOne<GameSession>().WithMany().HasForeignKey(e => e.GameSessionId)); 

    }
}