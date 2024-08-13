using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class GameSessionCharacterBuffConfiguration : IEntityTypeConfiguration<GameSessionCharacterBuff>
{
    public void Configure(EntityTypeBuilder<GameSessionCharacterBuff> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(GameSessionCharacterBuff));
        builder.HasBaseEntityInt();

        builder.PropertyWithUnderscore(x => x.Title);
        builder.PropertyWithUnderscore(x => x.Description);
        builder.PropertyWithUnderscore(x => x.BuffType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.IsNegative).HasDefaultValue(false);

        builder.HasData(new GameSessionCharacterBuff
        {
            Id = 1,
            Title = "Оглушение",
            Description = "Персонаж нейтрализован. Он не может совершать действия и пропускает ход",
            BuffType = Domain.Enums.BuffTypeEnum.Disable,
            IsNegative = true,
        });

        builder.HasData(new GameSessionCharacterBuff
        {
            Id = 2,
            Title = "Полет",
            Description = "Персонаж научился летать, теперь он может перемещаться по блокам доступных для полета",
            BuffType = Domain.Enums.BuffTypeEnum.Fly,
            IsNegative = false,
        });

        builder.HasData(new GameSessionCharacterBuff
        {
            Id = 3,
            Title = "Дополнительное действие",
            Description = "Персонаж получил дополнительные очки действия",
            BuffType = Domain.Enums.BuffTypeEnum.AddMainAction,
            IsNegative = false,
        });

        builder.HasData(new GameSessionCharacterBuff
        {
            Id = 4,
            Title = "Внушение",
            Description = "Персонаж внушен и должен выполнить заданные ему дейсвтия",
            BuffType = Domain.Enums.BuffTypeEnum.Suggestion,
            IsNegative = true,
        });
    }
}