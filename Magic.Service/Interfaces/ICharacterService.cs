using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Magic.Service.Interfaces
{
    public interface ICharacterService
    {
        Task<List<CharacterAvatar>> GetDefaultAvatar();
        Task<List<CharacterClass>> GetClasses();
        Task<List<CharacterCharacteristic>> GetCharacterCharacteristics();
    }
}
