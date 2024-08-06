using System.Text.Json;
using System.Text.Json.Serialization;
using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class CharacterTemplateConfiguration : IEntityTypeConfiguration<CharacterTemplate>
{
    public void Configure(EntityTypeBuilder<CharacterTemplate> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(CharacterTemplate));
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
        var serializeOptions = new JsonSerializerOptions();
        builder.PropertyWithUnderscore(x => x.Characteristics)
            .HasConversion(
                v => JsonSerializer.Serialize(v, serializeOptions),
                v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, serializeOptions)
            );


        builder.HasData(new CharacterTemplate
        {
            Id = Guid.Parse("1850beb4-ed84-4c7f-9a5c-cd7bce35e5d4"),
            Name = "Conductor Gennady",
            Description = "A real man with a real COCK",
            AvatarUrL = "https://cumm.co.uk/wp-content/uploads/2023/08/00-a-mans-cock.jpg?v=1698757203",
            CharacterClassId = CharacterClass.Warrior,
            AbilitieIds = new[]
            {
                1, 2, 3, 4, 5
            },
            Armor = 12,
            CharacterRaceId = CharacterRace.Human,
            MaxHP = 20,
            Speed = 8,
            Initiative = 5,
            Characteristics = new Dictionary<int, int>
            {
                { CharacterCharacteristic.Agility, 10 },
                { CharacterCharacteristic.Charisma, 20 },
                { CharacterCharacteristic.Intellect, 5 },
                { CharacterCharacteristic.Wisdom, 5 },
                { CharacterCharacteristic.Physique, 20 },
                { CharacterCharacteristic.Strength, 20 },
            }
        });

        builder.HasData(new CharacterTemplate
        {
            Id = Guid.Parse("ce12d784-19c8-4f07-be2c-06e0c853a30e"),
            Name = "Isabella The Lust",
            Description =
                "She had a baby but still remains virgin. Never had sex, but she is your mother. She is definitely not a whore",
            AvatarUrL =
                "https://source.boomplaymusic.com/group10/M00/02/07/40af9aa9b99e46aa8f205d25fe687fa9_320_320.jpg",
            CharacterClassId = CharacterClass.Priest,
            AbilitieIds = new[]
            {
                1, 10, 11, 12, 13
            },
            Armor = 8,
            CharacterRaceId = CharacterRace.Elf,
            MaxHP = 16,
            Speed = 8,
            Initiative = 3,
            Characteristics = new Dictionary<int, int>
            {
                { CharacterCharacteristic.Agility, 8 },
                { CharacterCharacteristic.Charisma, 20 },
                { CharacterCharacteristic.Intellect, 12 },
                { CharacterCharacteristic.Wisdom, 15 },
                { CharacterCharacteristic.Physique, 14 },
                { CharacterCharacteristic.Strength, 5 },
            }
        });

        builder.HasData(new CharacterTemplate
        {
            Id = Guid.Parse("dfc2813c-96c3-497e-8799-ad3aa9de0ae2"),
            Name = "Nolgroug Berylguard",
            Description = @"Background: Criminal
Motivation: You left home and found a place in a thieves' guild or some other criminal organization.
Origin: You always had a way with animals, able to calm them with a soothing word and a touch.
Events: 1
You fought in a battle. You escaped the battle unscathed, though many of your friends were injured or lost.",
            AvatarUrL =
                "https://source.boomplaymusic.com/group10/M00/02/07/40af9aa9b99e46aa8f205d25fe687fa9_320_320.jpg",
            CharacterClassId = CharacterClass.Hunter,
            AbilitieIds = new[]
            {
                1, 14, 15, 16, 17
            },
            Armor = 10,
            CharacterRaceId = CharacterRace.Dwarf,
            MaxHP = 18,
            Speed = 8,
            Initiative = 7,
            Characteristics = new Dictionary<int, int>
            {
                { CharacterCharacteristic.Agility, 16 },
                { CharacterCharacteristic.Charisma, 10 },
                { CharacterCharacteristic.Intellect, 10 },
                { CharacterCharacteristic.Wisdom, 10 },
                { CharacterCharacteristic.Physique, 16 },
                { CharacterCharacteristic.Strength, 10 },
            }
        });

        builder.HasData(new CharacterTemplate
        {
            Id = Guid.Parse("40078ee4-bfce-4ee3-b54b-ff6974e4bb69"),
            Name = "Snugug",
            Description = @"Background: Outlander
Motivation: From a young age, you couldn't abide the stink of the cities and preferred to spend your time in nature.
Origin: You grew up listening to tales of great wizards and knew you wanted to follow their path. You strove to be accepted at an academy of magic and succeeded.
Events: 2
You saw a demon and ran away before it could do anything to you.

You were accused of Assault. You were caught and convicted. You spent time in jail, chained to an oar, or performing hard labor. You served a sentence of 2 years or succeeded in escaping after that much time.",
            AvatarUrL =
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5DIPHOFPGgT1oS9B78rTLUv9RkVsWKnEccg&s",
            CharacterClassId = CharacterClass.Wizard,
            AbilitieIds = new[]
            {
                1, 6, 7, 8, 9
            },
            Armor = 6,
            CharacterRaceId = CharacterRace.Orc,
            MaxHP = 14,
            Speed = 8,
            Initiative = 4,
            Characteristics = new Dictionary<int, int>
            {
                { CharacterCharacteristic.Agility, 8 },
                { CharacterCharacteristic.Charisma, 10 },
                { CharacterCharacteristic.Intellect, 20 },
                { CharacterCharacteristic.Wisdom, 18 },
                { CharacterCharacteristic.Physique, 12 },
                { CharacterCharacteristic.Strength, 8 },
            }
        });
    }
}