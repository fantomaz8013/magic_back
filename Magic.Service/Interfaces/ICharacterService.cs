using Magic.Common.Models.Response;
using Magic.Domain.Entities;

namespace Magic.Service.Interfaces;

public interface ICharacterService
{
    Task<List<CharacterAvatar>> GetDefaultAvatar();
    Task<List<CharacterClass>> GetClasses();
    Task<List<CharacterCharacteristic>> GetCharacterCharacteristics();
    Task<List<CharacterRace>> GetCharacterRaces();
    Task<List<CharacterTemplateResponse>> GetCharacterTemplates();
    /// <summary>
    /// Прикрепить сопоставленный список юзеров и их выбраных персонажей к игровой сессии.
    /// </summary>
    /// <param name="userIdsToCharacterTemplatesIds">Dictionary(userId, characterTemplateId)</param>
    /// <param name="gameSessionId"></param>
    /// <returns></returns>
    Task<List<GameSessionCharacter>> ChooseCharacters(Dictionary<Guid, Guid> userIdsToCharacterTemplatesIds, Guid gameSessionId);
    /// <summary>
    /// Получить список выбранных персонажей в игровой сессии
    /// </summary>
    /// <param name="gameSessionId"></param>
    /// <returns></returns>
    Task<List<GameSessionCharacter>> GetGameSessionCharacters(Guid gameSessionId);
}