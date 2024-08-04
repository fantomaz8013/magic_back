using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations
{
    public class CharacterClassConfiguration : IEntityTypeConfiguration<CharacterClass>
    {
        public void Configure(EntityTypeBuilder<CharacterClass> builder)
        {
            builder.HasTableNameUnderscoreStyle(nameof(CharacterClass));
            builder.HasBaseEntityInt();

            builder.PropertyWithUnderscore(x => x.title);
            builder.PropertyWithUnderscore(x => x.characterCharacteristicId);
            builder.PropertyWithUnderscore(x => x.description);

            builder.HasForeignKey(x => x.characterCharacteristic, x => x.characterCharacteristicId);
            builder.HasOne(x => x.characterCharacteristic);

            builder.HasData(new CharacterClass 
            { 
                Id = CharacterClass.WARIOR, 
                title = "Воин",
                characterCharacteristicId = CharacterCharacteristic.STRENGTH,
                description = "Опытный гладиатор сражается на арене и хорошо знает, как использовать свои трезубец и сеть, чтобы опрокинуть противника и обойти его, вызывая ликование публики и получая тактическое преимущество",
            });
            builder.HasData(new CharacterClass
            {
                Id = CharacterClass.WIZARD,
                title = "Волшебник",
                characterCharacteristicId = CharacterCharacteristic.WISDOM,
                description = "Волшебники — адепты высшей магии, объединяющиеся по типу своих заклинаний. Опираясь на тонкие плетения магии, пронизывающей вселенную, волшебники способны создавать заклинания взрывного огня, искрящихся молний, тонкого обмана и грубого контроля над сознанием.",
            });
            builder.HasData(new CharacterClass
            {
                Id = CharacterClass.HUNTER,
                title = "Следопыт",
                characterCharacteristicId = CharacterCharacteristic.AGILITY,
                description = "Вдали от суеты городов и посёлков, за изгородями, которые защищают самые далёкие фермы от ужасов дикой природы, среди плотно стоящих деревьев, беспутья лесов и на просторах необъятных равнин следопыты несут свой бесконечный дозор.",
            });
            builder.HasData(new CharacterClass
            {
                Id = CharacterClass.PRIEST,
                title = "Жрец",
                characterCharacteristicId = CharacterCharacteristic.INTELLECT,
                description = "Жрецы являются посредниками между миром смертных и далёкими мирами богов. Настолько же разные, насколько боги, которым они служат, жрецы воплощают работу своих божеств. В отличие от обычного проповедника, жрец наделён божественной магией.",
            });
        }
    }
}
