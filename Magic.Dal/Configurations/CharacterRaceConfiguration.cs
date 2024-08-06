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

        builder.PropertyWithUnderscore(x => x.Title);
        builder.PropertyWithUnderscore(x => x.Description);

        builder.HasData(new CharacterRace
        { 
            Id = CharacterRace.Human, 
            Title = "Человек",
            Description = "В большинстве миров люди — это самая молодая из распространённых рас. Они поздно вышли на мировую сцену и живут намного меньше, чем дварфы, эльфы и драконы. Возможно, именно краткость их жизней заставляет их стремиться достигнуть как можно большего в отведённый им срок. А быть может, они хотят что-то доказать старшим расам, и поэтому создают могучие империи, основанные на завоеваниях и торговле. Что бы ни двигало ими, люди всегда были инноваторами и пионерами во всех мирах.",
        });
        builder.HasData(new CharacterRace
        {
            Id = CharacterRace.Elf,
            Title = "Эльф",
            Description = "Эльфы — это волшебный народ, обладающий неземным изяществом, живущий в мире, но не являющийся его частью. Они живут в местах, наполненных воздушной красотой, в глубинах древних лесов или в серебряных жилищах, увенчанных сверкающими шпилями и переливающихся волшебным светом. Там лёгкие дуновения ветра разносят обрывки тихих мелодий и нежные ароматы. Эльфы любят природу и магию, музыку и поэзию, и всё прекрасное, что есть в мире.",
        });
        builder.HasData(new CharacterRace
        {
            Id = CharacterRace.Dwarf,
            Title = "Дварф",
            Description = "Полные древнего величия королевства и вырезанные в толще гор чертоги, удары кирок и молотков, раздающиеся в глубоких шахтах и пылающий кузнечный горн, верность клану и традициям и пылающая ненависть к гоблинам и оркам — вот вещи, объединяющие всех дварфов.",
        });
        builder.HasData(new CharacterRace
        {
            Id = CharacterRace.Orc,
            Title = "Орк",
            Description = "Орки — дикие грабители и налетчики; у них сутулая осанка, низкий лоб и свиноподобные лица с выступающими нижними клыками, напоминающими бивни.",
        });
    }
}