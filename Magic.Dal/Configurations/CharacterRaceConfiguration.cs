using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class CharacterRaceConfiguration : IEntityTypeConfiguration<CharacterRace>
{
    public void Configure(EntityTypeBuilder<CharacterRace> builder)
    {
        builder.HasTableNameUnderscoreStyle(nameof(CharacterRace));
        builder.HasBaseEntityInt();

        builder.PropertyWithUnderscore(x => x.title);
        builder.PropertyWithUnderscore(x => x.description);

        builder.HasData(new CharacterRace
        { 
            Id = CharacterRace.HUMAN, 
            title = "Человек",
            description = "В большинстве миров люди — это самая молодая из распространённых рас. Они поздно вышли на мировую сцену и живут намного меньше, чем дварфы, эльфы и драконы. Возможно, именно краткость их жизней заставляет их стремиться достигнуть как можно большего в отведённый им срок. А быть может, они хотят что-то доказать старшим расам, и поэтому создают могучие империи, основанные на завоеваниях и торговле. Что бы ни двигало ими, люди всегда были инноваторами и пионерами во всех мирах.",
        });
        builder.HasData(new CharacterRace
        {
            Id = CharacterRace.ELF,
            title = "Эльф",
            description = "Эльфы — это волшебный народ, обладающий неземным изяществом, живущий в мире, но не являющийся его частью. Они живут в местах, наполненных воздушной красотой, в глубинах древних лесов или в серебряных жилищах, увенчанных сверкающими шпилями и переливающихся волшебным светом. Там лёгкие дуновения ветра разносят обрывки тихих мелодий и нежные ароматы. Эльфы любят природу и магию, музыку и поэзию, и всё прекрасное, что есть в мире.",
        });
        builder.HasData(new CharacterRace
        {
            Id = CharacterRace.DWARF,
            title = "Дварф",
            description = "Полные древнего величия королевства и вырезанные в толще гор чертоги, удары кирок и молотков, раздающиеся в глубоких шахтах и пылающий кузнечный горн, верность клану и традициям и пылающая ненависть к гоблинам и оркам — вот вещи, объединяющие всех дварфов.",
        });
        builder.HasData(new CharacterRace
        {
            Id = CharacterRace.ORC,
            title = "Орк",
            description = "Орки — дикие грабители и налетчики; у них сутулая осанка, низкий лоб и свиноподобные лица с выступающими нижними клыками, напоминающими бивни.",
        });
    }
}