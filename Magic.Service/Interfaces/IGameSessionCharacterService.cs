using Magic.Domain.Entities;

namespace Magic.Service.Interfaces;

public interface IGameSessionCharacterService
{
    /// <summary>
    /// Нанести урон персонажу
    /// </summary>
    /// <param name="gameSessionCharacterId">Идентификатор персонажа</param>
    /// <param name="value">Количество урона</param>
    /// <returns></returns>
    Task<GameSessionCharacter> Damage(Guid gameSessionCharacterId, int value);
    /// <summary>
    /// Вылечить персонажа
    /// </summary>
    /// <param name="gameSessionCharacterId">Идентификатор персонажа</param>
    /// <param name="value">Количество лечения</param>
    /// <returns></returns>
    Task<GameSessionCharacter> Heal(Guid gameSessionCharacterId, int value);

    /// <summary>
    /// Изменить персонажа
    /// </summary>
    /// <param name="gameSessionCharacter">Игровой персонаж</param>
    /// <param name="changedCharacterFields">Список изменяемых параметров и новые значения (gameSessionCharacterProperty, newValue)</param>
    /// <returns></returns>
    Task Change(
        GameSessionCharacter gameSessionCharacter,
        Dictionary<string, string> changedCharacterFields
    );

    /// <summary>
    /// Дать защиту персонажу. Дается поверх здоровья. При нанесении урона сначала будет вычтен этот параметр
    /// </summary>
    /// <param name="gameSessionCharacterId">Идентификатор персонажа</param>
    /// <param name="value">Количество защиты</param>
    /// <returns></returns>
    Task<GameSessionCharacter> Shield(Guid gameSessionCharacterId, int value);
    /// <summary>
    /// Задать позицию персонажа на игровой карте
    /// </summary>
    /// <param name="gameSessionCharacterId">Идентификатор персонажа</param>
    /// <param name="x">Х</param>
    /// <param name="y">У</param>
    /// <returns></returns>
    Task<GameSessionCharacter> SetPosition(Guid gameSessionCharacterId, int? x = null, int? y = null);
    /// <summary>
    /// Получить модификатор к броску кубика для конкретной характеристики
    /// </summary>
    /// <param name="gameSessionCharacterId">Идентификатор персонажа</param>
    /// <param name="characterCharacteristicId">Идентификатор характеристики</param>
    /// <returns></returns>
    Task<int> GetRollModificator(Guid gameSessionCharacterId, int characterCharacteristicId);
    /// <summary>
    /// Получить игрового персонажа
    /// </summary>
    /// <param name="gameSessionCharacterId">Идентификатор персонажа</param>
    /// <returns></returns>
    Task<GameSessionCharacter> GetGameSessionCharacter(Guid gameSessionCharacterId);
}