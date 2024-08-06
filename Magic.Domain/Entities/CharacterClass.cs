namespace Magic.Domain.Entities;

/// <summary>
/// Внутриигровой класс персонажа
/// </summary>
public class CharacterClass : BaseEntity<int>
{
    public const int Warrior = 1;
    public const int Wizard = 2;
    public const int Hunter = 3;
    public const int Priest = 4;

    /// <summary>
    /// Название класса
    /// </summary>
    public string Title { get; set; }

    public int CharacterCharacteristicId { get; set; }

    /// <summary>
    /// Основная характеристика класса. По ней будут совершаться проверки атаки 
    /// </summary>
    public CharacterCharacteristic CharacterCharacteristic { get; set; }

    /// <summary>
    /// Описание класса
    /// </summary>
    public string Description { get; set; }
}