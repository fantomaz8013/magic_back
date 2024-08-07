using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class BaseGameSessionMessageConfiguration : IEntityTypeConfiguration<BaseGameSessionMessage>
{
    public void Configure(EntityTypeBuilder<BaseGameSessionMessage> builder)
    {
        builder.HasTableNameUnderscoreStyle("GameSessionMessages");
        builder.HasBaseEntityGuid();

        builder
            .HasDiscriminator(x => x.GameSessionMessageTypeEnum)
            .HasValue<ServerGameSessionMessage>(GameSessionMessageTypeEnum.Server)
            .HasValue<ChatGameGameSessionMessage>(GameSessionMessageTypeEnum.Chat)
            .HasValue<DiceGameSessionMessage>(GameSessionMessageTypeEnum.Dice);

        builder.HasCreatedDateEntity();

        builder.PropertyWithUnderscore(x => x.GameSessionMessageTypeEnum)
            .HasConversion<int>();

        builder.PropertyWithUnderscore(x => x.GameSessionId);
        builder.HasForeignKey(x => x.GameSession, x => x.GameSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.GameSession);
    }
}