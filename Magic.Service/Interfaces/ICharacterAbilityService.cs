using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Magic.Service.Interfaces;

public interface ICharacterAbilityService
{
    Task<ApplyAbilityResponse> ApplyAbility(int characterAbilityId, Guid casterGameSessionCharacterId, int x, int y);
    Task<List<CharacterAbility>> GetAbilities();
}