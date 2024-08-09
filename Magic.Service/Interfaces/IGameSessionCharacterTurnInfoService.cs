using Magic.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Magic.Service.Interfaces;

public interface IGameSessionCharacterTurnInfoService
{
    /// <summary>
    /// Получить информацию о ходе персонажа. Инициализирует, если до этого не было информации
    /// </summary>
    /// <param name="gameSessionCharacterId"></param>
    /// <returns></returns>
    Task<GameSessionCharacterTurnInfo> GetCharacterTurnInfo(Guid gameSessionCharacterId);
    /// <summary>
    /// Сброс информации о ходе. Сбрасывает все перезарядки способностей, если это возможно
    /// </summary>
    /// <param name="gameSessionCharacterId"></param>
    /// <returns></returns>
    Task<GameSessionCharacterTurnInfo> RestoreCharacterTurnInfo(Guid gameSessionCharacterId);
    /// <summary>
    /// Обновляет информацию о ходе. Вызывать, когда персонаж сделал свой ход
    /// </summary>
    /// <param name="gameSessionCharacterId"></param>
    /// <returns></returns>
    Task<GameSessionCharacterTurnInfo> UpdateTurnInfo(Guid gameSessionCharacterId);
}