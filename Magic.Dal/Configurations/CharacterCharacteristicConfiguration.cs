using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations
{
    public class CharacterCharacteristicConfiguration : IEntityTypeConfiguration<CharacterCharacteristic>
    {
        public void Configure(EntityTypeBuilder<CharacterCharacteristic> builder)
        {
            builder.HasTableNameUnderscoreStyle(nameof(CharacterCharacteristic));
            builder.HasBaseEntityInt();

            builder.PropertyWithUnderscore(x => x.title);
            builder.PropertyWithUnderscore(x => x.description);

            builder.HasData(new CharacterCharacteristic 
            { 
                Id = CharacterCharacteristic.STRENGTH, 
                title = "Сила",
                description = "Проверки Силы могут моделировать попытки поднять, толкнуть, подтянуть или сломать что-то, попытки втиснуть своё тело в некое пространство или другие попытки применения грубой силы. Навык Атлетика отражает особую склонность к некоторым проверкам Силы."
            });
            builder.HasData(new CharacterCharacteristic
            {
                Id = CharacterCharacteristic.AGILITY,
                title = "Ловкость",
                description = "Проверка Ловкости может моделировать любую попытку перемещаться ловко, быстро или тихо, либо попытку не упасть с шаткой опоры. Навыки Акробатика, Ловкость рук и Скрытность отражают особую склонность к некоторым проверкам Ловкости."
            });
            builder.HasData(new CharacterCharacteristic
            {
                Id = CharacterCharacteristic.PHYSIQUE,
                title = "Телосложение",
                description = "Проверки Телосложения совершаются не часто, и от него не зависят никакие навыки, так как выносливость, которую отражает эта характеристика, пассивна, и персонаж или чудовище не может активно её использовать. Однако проверка Телосложения может моделировать вашу попытку сделать что-то необычное."
            });
            builder.HasData(new CharacterCharacteristic
            {
                Id = CharacterCharacteristic.INTELLECT,
                title = "Интеллект",
                description = "Проверки Интеллекта происходят когда вы используете логику, образование, память или дедуктивное мышление. Навыки История, Магия, Природа, Расследование и Религия отражают особую склонность к некоторым проверкам Интеллекта."
            });
            builder.HasData(new CharacterCharacteristic
            {
                Id = CharacterCharacteristic.WISDOM,
                title = "Мудрость",
                description = "Проверки Мудрости могут отражать попытки понять язык тела, понять чьи-то переживания, заметить что-то в окружающем мире или позаботиться о раненом. Навыки Восприятие, Выживание, Медицина, Проницательность и Уход за животными отражают особую склонность к некоторым проверкам Мудрости."
            });
            builder.HasData(new CharacterCharacteristic
            {
                Id = CharacterCharacteristic.CHARISMA,
                title = "Харизма",
                description = "Проверку Харизмы можно совершать при попытке повлиять на других или развлечь их, когда вы пытаетесь произвести впечатление или убедительно соврать, или если вы пытаетесь разобраться в сложной социальной ситуации. Навыки Выступление, Запугивание, Обман и Убеждение отражают особую склонность к некоторым проверкам Харизмы."
            });
        }
    }
}
