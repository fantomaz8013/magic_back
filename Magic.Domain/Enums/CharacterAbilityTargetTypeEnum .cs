namespace Magic.Domain.Enums;

/// <summary>
/// Тип применения способности.
/// </summary>
public enum CharacterAbilityTargetTypeEnum
{
    /// <summary>
    /// Способность должна применяться на конкретную цель
    /// </summary>
    Targert = 1,
    /// <summary>
    /// Целью способности должна быть область
    /// </summary>
    Area = 2,
    /// <summary>
    /// Способность должна применяться на себя
    /// </summary>
    TargertSelf = 3,
    /// <summary>
    /// Способность должна применяться по конусу
    /// </summary>
    Cone = 4,
}