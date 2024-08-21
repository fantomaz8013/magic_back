using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Magic.DAL.Configurations;

public class TilePropertyConfiguration : IEntityTypeConfiguration<TileProperty>
{
    public void Configure(EntityTypeBuilder<TileProperty> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(TileProperty));
        builder.HasBaseEntityInt();

        builder.PropertyWithUnderscore(x => x.Title);
        builder.PropertyWithUnderscore(x => x.Description);
        builder.PropertyWithUnderscore(x => x.Image);
        builder.PropertyWithUnderscore(x => x.PenaltyValue);
        builder.PropertyWithUnderscore(x => x.CollisionType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.PenaltyType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.TargetType).HasConversion<int>();

        builder.HasOne(x => x.TilePropertyIfDestroyed);
        builder.HasForeignKey(x => x.TilePropertyIfDestroyed, x => x.TilePropertyIdIfDestroyed);


        builder.HasData(new TileProperty
        {
            Id = 1,
            Title = "Земля",
            Description = "Самая обычная земля",
            Image = "storage/tiles/1.jpg",
            CollisionType = Domain.Enums.TilePropertyCollisionTypeEnum.None,
            PenaltyType = Domain.Enums.TilePropertyPenaltyTypeEnum.None,
            TargetType = Domain.Enums.TilePropertyTargetTypeEnum.None,
            PenaltyValue = null,
            Health = null,
        });

        builder.HasData(new TileProperty
        {
            Id = 2,
            Title = "Каменая стена",
            Description = "Самая обычная каменая стена. Невозможно пройти, но возможно сломать.",
            Image = "storage/tiles/2.png",
            CollisionType = Domain.Enums.TilePropertyCollisionTypeEnum.OnlyFly,
            PenaltyType = Domain.Enums.TilePropertyPenaltyTypeEnum.None,
            TargetType = Domain.Enums.TilePropertyTargetTypeEnum.Destructible,
            PenaltyValue = null,
            Health = 5,
            TilePropertyIdIfDestroyed = 1,
        });

        builder.HasData(new TileProperty
        {
            Id = 3,
            Title = "Огонь",
            Description = "Горит блять! Не ходи сцука сюда!",
            Image = "storage/tiles/3.png",
            CollisionType = Domain.Enums.TilePropertyCollisionTypeEnum.None,
            PenaltyType = Domain.Enums.TilePropertyPenaltyTypeEnum.PenaltyHealth,
            TargetType = Domain.Enums.TilePropertyTargetTypeEnum.None,
            PenaltyValue = 5,
            Health = null,
        });

        builder.HasData(new TileProperty
        {
            Id = 4,
            Title = "Вода",
            Description = "Тут не горит, но ходить сложно",
            Image = "storage/tiles/4.png",
            CollisionType = Domain.Enums.TilePropertyCollisionTypeEnum.None,
            PenaltyType = Domain.Enums.TilePropertyPenaltyTypeEnum.PenaltySpeed,
            TargetType = Domain.Enums.TilePropertyTargetTypeEnum.None,
            PenaltyValue = 1,
            Health = null,
        });

        builder.HasData(new TileProperty
        {
            Id = 5,
            Title = "Трава",
            Description = "Самая обычная трава ( хих ) ",
            Image = "storage/tiles/5.png",
            CollisionType = Domain.Enums.TilePropertyCollisionTypeEnum.None,
            PenaltyType = Domain.Enums.TilePropertyPenaltyTypeEnum.None,
            TargetType = Domain.Enums.TilePropertyTargetTypeEnum.None,
            PenaltyValue = null,
            Health = null,
        });
    }
}