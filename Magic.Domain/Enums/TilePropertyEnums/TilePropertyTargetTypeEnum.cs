namespace Magic.Domain.Enums;

/// <summary>
/// Тип возможного действия по отношению к тайлу
/// </summary>
public enum TilePropertyTargetTypeEnum
{
    /// <summary>
    /// Тайл нельзя выбрать в таргет
    /// </summary>
    None = 1,
    /// <summary>
    /// Разрушаемый
    /// </summary>
    Destructible = 2,
}