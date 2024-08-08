namespace Magic.Domain.Enums;

/// <summary>
/// Тип штрафа за проход по тайлу
/// </summary>
public enum TilePropertyPenaltyTypeEnum
{
    /// <summary>
    /// Нет штрафа за проход по клетке
    /// </summary>
    None = 1,
    /// <summary>
    /// Штраф в виде жизней
    /// </summary>
    PenaltyHealth = 2,
    /// <summary>
    /// Штраф в виде скорости
    /// </summary>
    PenaltySpeed = 3,
}