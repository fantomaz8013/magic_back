using Magic.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Magic.Service.Interfaces;

public interface IGameSessionCharacterService
{
    Task<GameSessionCharacter> Damage(Guid gameSessionCharacterId, int value);
    Task<GameSessionCharacter> Heal(Guid gameSessionCharacterId, int value);
    Task<GameSessionCharacter> Armor(Guid gameSessionCharacterId, int value);
    Task<GameSessionCharacter> SetPosition(Guid gameSessionCharacterId, int? x = null, int? y = null);
    Task<int> GetRollModificator(Guid gameSessionCharacterId, int characterCharacteristicId);
}