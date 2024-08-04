using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Magic.Domain.Enums;

namespace Magic.DAL.Configurations
{
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
            builder.PropertyWithUnderscore(x => x.title);
            builder.PropertyWithUnderscore(x => x.description);
            builder.PropertyWithUnderscore(x => x.distance);
            builder.PropertyWithUnderscore(x => x.cubeCount);
            builder.PropertyWithUnderscore(x => x.coolDownCount);

            builder.PropertyWithUnderscore(x => x.characterClassId).IsRequired(false).HasDefaultValue(null);
            builder.HasForeignKey(x => x.CharacterClass, x => x.characterClassId);
            builder.HasOne(x => x.CharacterClass);

            builder.PropertyWithUnderscore(x => x.casterCharacterCharacteristicId).IsRequired(false).HasDefaultValue(null);
            builder.HasForeignKey(x => x.CasterCharacterCharacteristic, x => x.casterCharacterCharacteristicId);
            builder.HasOne(x => x.CasterCharacterCharacteristic);

            builder.PropertyWithUnderscore(x => x.targetCharacterCharacteristicId).IsRequired(false).HasDefaultValue(null);
            builder.HasForeignKey(x => x.TargetCharacterCharacteristic, x => x.targetCharacterCharacteristicId);
            builder.HasOne(x => x.TargetCharacterCharacteristic);
            
            builder.HasData(new CharacterAbility
            {
                Id = 1,
                title = "Удар основным оружием",
                description = "Вы наносите урон основным оружием по выбраной цели нанося 1к10 урона",
                Type = CharacterAbilityTypeEnum.Attack,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.None,
                CubeType = CubeTypeEnum.D10,
                distance = 2,
                cubeCount = 1,
            });

            builder.HasData(new CharacterAbility 
            {
                Id = 2, 
                title = "Второе дыхание",
                description = "Вы обладаете ограниченным источником выносливости, которым можете воспользоваться, чтобы уберечь себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10",
                Type = CharacterAbilityTypeEnum.Healing,
                TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
                ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                characterClassId = CharacterClass.WARIOR,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 3,
                title = "Порыв к действию",
                description = "Немедленно получите ещё одно действие в этом ходу. На следующий ход эффект порыва исчезает",
                Type = CharacterAbilityTypeEnum.Buff,
                TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
                ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                characterClassId = CharacterClass.WARIOR,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 4,
                title = "Размашистый удар",
                description = "Удар основным оружием по конусу 3 клетки перед собой. Наносит 1к10 всем, кто находится в конусе",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Cone,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                distance = 1,
                radius = 3,
                characterClassId = CharacterClass.WARIOR,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 5,
                title = "Ярость",
                description = "Немедленно получите еще 3 очка действия в этом ходу, но получите 2к6 урона по себе",
                Type = CharacterAbilityTypeEnum.Buff,
                TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
                ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
                CubeType = CubeTypeEnum.D6,
                cubeCount = 2,
                characterClassId = CharacterClass.WARIOR,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 6,
                title = "Огненый шар",
                description = "Немедленно выпускает огненый шар в точку и происходит взрыв с радиюусов 1м. Дальность 30м. Все существа в радиусе взрыва получают 3к10 урона",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Area,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
                distance = 30,
                radius = 1,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 3,
                characterClassId = CharacterClass.WIZARD,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 7,
                title = "Внушение",
                description = "Вы внушаете определенный курс действий (ограниченный одной-двумя фразами) существу, видимому в пределах дистанции, способному слышать и понимать вас",
                Type = CharacterAbilityTypeEnum.DeBuff,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                distance = 30,
                CubeType = CubeTypeEnum.D20,
                cubeCount = 1,
                targetCharacterCharacteristicId = CharacterCharacteristic.WISDOM,
                characterClassId = CharacterClass.WIZARD,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 8,
                title = "Огненный снаряд",
                description = "Вы кидаете сгусток огня в существо или предмет в пределах дистанции ( 30 м ). Совершите по цели дальнобойную атаку заклинанием. При попадании цель получает урон огнём 1к10.",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.None,
                distance = 30,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                characterClassId = CharacterClass.WIZARD,
                casterCharacterCharacteristicId = CharacterCharacteristic.WISDOM,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 9,
                title = "Левитация",
                description = "Выберите точку и перелетите к ней игнорируя все препятствия",
                Type = CharacterAbilityTypeEnum.Buff,
                TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                distance = 10,
                characterClassId = CharacterClass.WIZARD,
            });


            builder.HasData(new CharacterAbility
            {
                Id = 10,
                title = "Исцеление",
                description = "Существо, которого вы касаетесь, восстанавливает количество хитов, равное 1к8",
                Type = CharacterAbilityTypeEnum.Healing,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                distance = 2,
                CubeType = CubeTypeEnum.D8,
                cubeCount = 1,
                characterClassId = CharacterClass.PRIEST,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 11,
                title = "Оглушающая кара",
                description = "Вы выпускаете сгусток светлой энергии по противнику, наносящий 1к8 урона и оглушающий его на 1 ход",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                distance = 30,
                CubeType = CubeTypeEnum.D8,
                cubeCount = 1,
                characterClassId = CharacterClass.PRIEST,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 12,
                title = "Воскрешение",
                description = "Вы можете воскресить павшего союзника c 1к20",
                Type = CharacterAbilityTypeEnum.Healing,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
                distance = 30,
                CubeType = CubeTypeEnum.D20,
                cubeCount = 1,
                characterClassId = CharacterClass.PRIEST,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 13,
                title = "Божественный щит",
                description = "Вы накладываете на существо божественный щит, способный похлотить 1к10 урона",
                Type = CharacterAbilityTypeEnum.Protection,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                distance = 30,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                characterClassId = CharacterClass.PRIEST,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 14,
                title = "Залп стрел",
                description = "Выпускает град стрел по указаной области, нанося всем существам 1к10 урона",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Area,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                distance = 30,
                radius = 1,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                characterClassId = CharacterClass.HUNTER,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 15,
                title = "Перевязка ран",
                description = "Вы обладаете бинтами, которым можете воспользоваться, чтобы исцелить себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10",
                Type = CharacterAbilityTypeEnum.Healing,
                TargetType = CharacterAbilityTargetTypeEnum.TargertSelf,
                ActionType = CharacterAbilityActionTypeEnum.AdditionalAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.AfterFight,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                characterClassId = CharacterClass.HUNTER,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 16,
                title = "Точный выстрел",
                description = "Вы стреляете из лука по цели, нанося 1к10 урона",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.None,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 1,
                distance = 30,
                characterClassId = CharacterClass.HUNTER,
            });

            builder.HasData(new CharacterAbility
            {
                Id = 17,
                title = "Выстрел адамантиевой стрелой",
                description = "Вы стреляете из лука по цели особой стрелой, нанося 5к10 урона",
                Type = CharacterAbilityTypeEnum.Attack,
                TargetType = CharacterAbilityTargetTypeEnum.Targert,
                ActionType = CharacterAbilityActionTypeEnum.MainAction,
                CoolDownType = CharacterAbilityCoolDownTypeEnum.OnePerGame,
                CubeType = CubeTypeEnum.D10,
                cubeCount = 5,
                distance = 30,
                characterClassId = CharacterClass.HUNTER,
            });

        }
    }
}
