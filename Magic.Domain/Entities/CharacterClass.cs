using Magic.Domain.Enums;

namespace Magic.Domain.Entities
{
    /// <summary>
    /// Внутриигровой класс персонажа
    /// </summary>
    public class CharacterClass : BaseEntity<int>
    {
        public const int WARIOR = 1;
        public const int WIZARD = 2;
        public const int HUNTER = 3;
        public const int PRIEST = 4;
        /// <summary>
        /// Название класса
        /// </summary>
        public string title { get; set; }
        
        public int characterCharacteristicId { get; set; }
        /// <summary>
        /// Основная характеристика класса. По ней будут совершаться проверки атаки 
        /// </summary>
        public CharacterCharacteristic characterCharacteristic { get; set; }
        /// <summary>
        /// Описание класса
        /// </summary>
        public string description {  get; set; }
    }
}
