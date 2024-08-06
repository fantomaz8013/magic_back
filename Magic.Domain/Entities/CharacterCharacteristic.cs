namespace Magic.Domain.Entities;

/// <summary>
/// Характеристика персонажа
/// </summary>
public class CharacterCharacteristic : BaseEntity<int>
{
    // DO NOT EVER CHANGE THE ORDER
    public const int Strength  = 1;
    public const int Agility   = 2;
    public const int Physique  = 3;
    public const int Intellect = 4;
    public const int Wisdom    = 5;
    public const int Charisma  = 6;
    /// <summary>
    /// Название характеристики
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Описание характеристики ( для чего приминяется и т.д )
    /// </summary>
    public string Description { get; set; }
}