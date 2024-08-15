using System.Text.Json;
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
            Name = "Ардан Громовержец",
            Description = "Ардан Громовержец — бесстрашный воин, владеющий молотом, вызывающим гром. Его мощь и отвага легендарны, а враги трепещут перед его яростью в бою.",
            AvatarUrL = "storage/character/avatar/1.png",
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
            Name = "Каэл Светоносный",
            Description =
                "Каэл Светоносный — мудрый жрец, исцеляющий раны и изгоняющий тьму. Его сила исходит от древних богов, а сердце наполнено милосердием.",
            AvatarUrL =
                "storage/character/avatar/2.png",
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
            Name = "Тарен Лесной Страж",
            Description = @"Тарен Лесной Страж — искусный охотник, владеющий луком и кинжалом. Он незаметно передвигается по лесу и всегда попадает в цель.",
            AvatarUrL =
                "storage/character/avatar/3.png",
            CharacterClassId = CharacterClass.Hunter,
            AbilitieIds = new[]
            {
                1, 14, 15, 16, 17
            },
            Armor = 10,
            CharacterRaceId = CharacterRace.Elf,
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
            Name = "Элриан Заклинатель",
            Description = @"Элриан Заклинатель — могущественный волшебник, повелевающий стихиями. Его знания древних магий и заклинаний делают его непревзойденным в борьбе с темными силами.",
            AvatarUrL =
                "storage/character/avatar/4.png",
            CharacterClassId = CharacterClass.Wizard,
            AbilitieIds = new[]
            {
                1, 6, 7, 8, 9
            },
            Armor = 6,
            CharacterRaceId = CharacterRace.Human,
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