namespace Magic.Domain.Entities;

/// <summary>
/// Характеристика персонажа
/// </summary>
public class CharacterCharacteristic : BaseEntity<int>
{
    public const int STRENGTH  = 1;
    public const int AGILITY   = 2;
    public const int PHYSIQUE  = 3;
    public const int INTELLECT = 4;
    public const int WISDOM    = 5;
    public const int CHARISMA  = 6;
    /// <summary>
    /// Название характеристики
    /// </summary>
    public string title { get; set; }
    /// <summary>
    /// Описание характеристики ( для чего приминяется и т.д )
    /// </summary>
    public string description { get; set; }
}