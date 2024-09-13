using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Magic.Domain.Enums;

namespace Magic.DAL.Configurations;

public class CharacterAbilityConfiguration : IEntityTypeConfiguration<CharacterAbility>
{
    public void Configure(EntityTypeBuilder<CharacterAbility> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(CharacterAbility));
        builder.HasBaseEntityInt();

        builder.PropertyWithUnderscore(x => x.Type).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.TargetType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.CubeType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.ActionType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.CoolDownType).HasConversion<int>();
        builder.PropertyWithUnderscore(x => x.Title);
        builder.PropertyWithUnderscore(x => x.Radius);
        builder.PropertyWithUnderscore(x => x.Description);
        builder.PropertyWithUnderscore(x => x.Distance);
        builder.PropertyWithUnderscore(x => x.CubeCount);
        builder.PropertyWithUnderscore(x => x.CoolDownCount);
        builder.PropertyWithUnderscore(x => x.Icons);

        builder.PropertyWithUnderscore(x => x.CharacterClassId).IsRequired(false).HasDefaultValue(null);
        builder.HasForeignKey(x => x.CharacterClass, x => x.CharacterClassId);
        builder.HasOne(x => x.CharacterClass);

        builder.PropertyWithUnderscore(x => x.CharacterBuffId).IsRequired(false).HasDefaultValue(null);
        builder.HasForeignKey(x => x.CharacterBuff, x => x.CharacterBuffId);
        builder.HasOne(x => x.CharacterBuff);

        builder.PropertyWithUnderscore(x => x.CasterCharacterCharacteristicId).IsRequired(false).HasDefaultValue(null);
        builder.HasForeignKey(x => x.CasterCharacterCharacteristic, x => x.CasterCharacterCharacteristicId);
        builder.HasOne(x => x.CasterCharacterCharacteristic);

        builder.PropertyWithUnderscore(x => x.TargetCharacterCharacteristicId).IsRequired(false).HasDefaultValue(null);
        builder.HasForeignKey(x => x.TargetCharacterCharacteristic, x => x.TargetCharacterCharacteristicId);
        builder.HasOne(x => x.TargetCharacterCharacteristic);
            
        builder.HasData(new CharacterAbility
        {
            Id = 1,
            Title = "Удар основным оружием",
            Description = "Вы наносите урон основным оружием по выбраной цели нанося 1к10 урона",
            Type = CharacterAbilityTypeEnum.Attack,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.None,
            CubeType = CubeTypeEnum.D10,
            Distance = 2,
            CubeCount = 1,
            Icons = "storage/icons/ability/1.png",
        });

        builder.HasData(new CharacterAbility 
        {
            Id = 2, 
            Title = "Второе дыхание",
            Description = "Вы обладаете ограниченным источником выносливости, которым можете воспользоваться, чтобы уберечь себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10",
            Type = CharacterAbilityTypeEnum.Healing,
            TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
            ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Warrior,
            Icons = "storage/icons/ability/2.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 3,
            Title = "Порыв к действию",
            Description = "Немедленно получите ещё одно действие в этом ходу. На следующий ход эффект порыва исчезает",
            Type = CharacterAbilityTypeEnum.Buff,
            TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
            ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            CharacterClassId = CharacterClass.Warrior,
            BuffCount = 1,
            CharacterBuffId = 3,
            Icons = "storage/icons/ability/3.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 4,
            Title = "Размашистый удар",
            Description = "Удар основным оружием по конусу 3 клетки перед собой. Наносит 1к10 всем, кто находится в конусе",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Cone,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            Distance = 1,
            Radius = 3,
            CharacterClassId = CharacterClass.Warrior,
            Icons = "storage/icons/ability/4.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 5,
            Title = "Ярость",
            Description = "Немедленно получите еще 3 очка действия в этом ходу, но получите 2к6 урона по себе",
            Type = CharacterAbilityTypeEnum.Buff,
            TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
            ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
            CubeType = CubeTypeEnum.D6,
            CubeCount = 2,
            CharacterClassId = CharacterClass.Warrior,
            BuffCount = 3,
            CharacterBuffId = 3,
            Icons = "storage/icons/ability/5.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 6,
            Title = "Огненый шар",
            Description = "Немедленно выпускает огненый шар в точку и происходит взрыв с радиюусов 1м. Дальность 30м. Все существа в радиусе взрыва получают 3к10 урона",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Area,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
            Distance = 30,
            Radius = 1,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 3,
            CharacterClassId = CharacterClass.Wizard,
            Icons = "storage/icons/ability/6.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 7,
            Title = "Внушение",
            Description = "Вы внушаете определенный курс действий (ограниченный одной-двумя фразами) существу, видимому в пределах дистанции, способному слышать и понимать вас",
            Type = CharacterAbilityTypeEnum.DeBuff,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            Distance = 30,
            CubeType = CubeTypeEnum.D20,
            CubeCount = 1,
            TargetCharacterCharacteristicId = CharacterCharacteristic.Wisdom,
            CharacterClassId = CharacterClass.Wizard,
            BuffCount = 1,
            CharacterBuffId = 4,
            Icons = "storage/icons/ability/7.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 8,
            Title = "Огненный снаряд",
            Description = "Вы кидаете сгусток огня в существо или предмет в пределах дистанции ( 30 м ). Совершите по цели дальнобойную атаку заклинанием. При попадании цель получает урон огнём 1к10.",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.None,
            Distance = 30,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Wizard,
            CasterCharacterCharacteristicId = CharacterCharacteristic.Wisdom,
            Icons = "storage/icons/ability/8.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 9,
            Title = "Левитация",
            Description = "Выберите точку и перелетите к ней игнорируя все препятствия",
            Type = CharacterAbilityTypeEnum.Buff,
            TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            Distance = 10,
            CharacterClassId = CharacterClass.Wizard,
            BuffCount = 1,
            CharacterBuffId = 2,
            Icons = "storage/icons/ability/9.png",
        });


        builder.HasData(new CharacterAbility
        {
            Id = 10,
            Title = "Исцеление",
            Description = "Существо, которого вы касаетесь, восстанавливает количество хитов, равное 1к8",
            Type = CharacterAbilityTypeEnum.Healing,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            Distance = 2,
            CubeType = CubeTypeEnum.D8,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Priest,
            Icons = "storage/icons/ability/10.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 11,
            Title = "Оглушающая кара",
            Description = "Вы выпускаете сгусток светлой энергии по противнику, наносящий 1к8 урона и оглушающий его на 1 ход",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            Distance = 30,
            CubeType = CubeTypeEnum.D8,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Priest,
            BuffCount = 1,
            CharacterBuffId = 1,
            Icons = "storage/icons/ability/11.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 12,
            Title = "Воскрешение",
            Description = "Вы можете воскресить павшего союзника c 1к20",
            Type = CharacterAbilityTypeEnum.Healing,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
            Distance = 30,
            CubeType = CubeTypeEnum.D20,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Priest,
            Icons = "storage/icons/ability/12.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 13,
            Title = "Божественный щит",
            Description = "Вы накладываете на существо божественный щит, способный поглотить 1к10 урона",
            Type = CharacterAbilityTypeEnum.Protection,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            Distance = 30,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Priest,
            Icons = "storage/icons/ability/13.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 14,
            Title = "Залп стрел",
            Description = "Выпускает град стрел по указаной области, нанося всем существам 1к10 урона",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Area,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            Distance = 30,
            Radius = 1,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Hunter,
            Icons = "storage/icons/ability/14.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 15,
            Title = "Перевязка ран",
            Description = "Вы обладаете бинтами, которым можете воспользоваться, чтобы исцелить себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10",
            Type = CharacterAbilityTypeEnum.Healing,
            TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
            ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            CharacterClassId = CharacterClass.Hunter,
            Icons = "storage/icons/ability/15.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 16,
            Title = "Точный выстрел",
            Description = "Вы стреляете из лука по цели, нанося 1к10 урона",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.None,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 1,
            Distance = 30,
            CharacterClassId = CharacterClass.Hunter,
            Icons = "storage/icons/ability/16.png",
        });

        builder.HasData(new CharacterAbility
        {
            Id = 17,
            Title = "Выстрел адамантиевой стрелой",
            Description = "Вы стреляете из лука по цели особой стрелой, нанося 5к10 урона",
            Type = CharacterAbilityTypeEnum.Attack,
            TargetType = CharacterAbilityTargetTypeEnum.Target,
            ActionType = CharacterAbilityActionTypeEnum.MainAction,
            CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
            CubeType = CubeTypeEnum.D10,
            CubeCount = 5,
            Distance = 30,
            CharacterClassId = CharacterClass.Hunter,
            Icons = "storage/icons/ability/17.png",
        });

    }
}