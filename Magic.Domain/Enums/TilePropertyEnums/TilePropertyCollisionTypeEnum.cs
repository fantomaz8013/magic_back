namespace Magic.Domain.Enums;

/// <summary>
/// Тип коллизии тайла
/// </summary>
public enum TilePropertyCollisionTypeEnum
{
    /// <summary>
    /// Свободный проход по клетке
    /// </summary>
    None = 1,
    /// <summary>
    /// Движение через этот тайл невозможно
    /// </summary>
    NoMove = 2,
    /// <summary>
    /// Движение только с полетом
    /// </summary>
    OnlyFly = 3,
}