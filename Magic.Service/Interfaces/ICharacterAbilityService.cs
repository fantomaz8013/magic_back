using Magic.Common.Models.Response;
using Microsoft.AspNetCore.Http;

namespace Magic.Service.Interfaces;

public interface ICharacterAbilityService
{
    Task<ApplyAbilityResponse> ApplyAbility(int characterAbilityId, Guid casterGameSessionCharacterId, int x, int y);
}