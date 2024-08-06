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
}