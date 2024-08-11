namespace Magic.Domain.Enums;

/// <summary>
/// Тип перезарядки способности
/// </summary>
public enum CharacterAbilityCoolDownTypeEnum
{
    /// <summary>
    /// Способность перезаряжается во время драки.
    /// </summary>
    InFight = 1,
    /// <summary>
    /// Способность перезаряжается после драки
    /// </summary>
    AfterFight = 2,
    /// <summary>
    /// Способность не имеет перезарядку. Можно использовать каждый ход
    /// </summary>
    None = 3,
    /// <summary>
    /// Способность можно использовать 1 раз за всю игру
    /// </summary>
    OnePerGame = 4,
}