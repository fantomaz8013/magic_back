using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class CharacterClassConfiguration : IEntityTypeConfiguration<CharacterClass>
{
    public void Configure(EntityTypeBuilder<CharacterClass> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(CharacterClass));
        builder.HasBaseEntityInt();

        builder.PropertyWithUnderscore(x => x.Title);
        builder.PropertyWithUnderscore(x => x.CharacterCharacteristicId);
        builder.PropertyWithUnderscore(x => x.Description);

        builder.HasForeignKey(x => x.CharacterCharacteristic, x => x.CharacterCharacteristicId);
        builder.HasOne(x => x.CharacterCharacteristic);

        builder.HasData(new CharacterClass 
        { 
            Id = CharacterClass.Warrior, 
            Title = "Воин",
            CharacterCharacteristicId = CharacterCharacteristic.Strength,
            Description = "Опытный гладиатор сражается на арене и хорошо знает, как использовать свои трезубец и сеть, чтобы опрокинуть противника и обойти его, вызывая ликование публики и получая тактическое преимущество",
        });
        builder.HasData(new CharacterClass
        {
            Id = CharacterClass.Wizard,
            Title = "Волшебник",
            CharacterCharacteristicId = CharacterCharacteristic.Wisdom,
            Description = "Волшебники — адепты высшей магии, объединяющиеся по типу своих заклинаний. Опираясь на тонкие плетения магии, пронизывающей вселенную, волшебники способны создавать заклинания взрывного огня, искрящихся молний, тонкого обмана и грубого контроля над сознанием.",
        });
        builder.HasData(new CharacterClass
        {
            Id = CharacterClass.Hunter,
            Title = "Следопыт",
            CharacterCharacteristicId = CharacterCharacteristic.Agility,
            Description = "Вдали от суеты городов и посёлков, за изгородями, которые защищают самые далёкие фермы от ужасов дикой природы, среди плотно стоящих деревьев, беспутья лесов и на просторах необъятных равнин следопыты несут свой бесконечный дозор.",
        });
        builder.HasData(new CharacterClass
        {
            Id = CharacterClass.Priest,
            Title = "Жрец",
            CharacterCharacteristicId = CharacterCharacteristic.Intellect,
            Description = "Жрецы являются посредниками между миром смертных и далёкими мирами богов. Настолько же разные, насколько боги, которым они служат, жрецы воплощают работу своих божеств. В отличие от обычного проповедника, жрец наделён божественной магией.",
        });
    }
}