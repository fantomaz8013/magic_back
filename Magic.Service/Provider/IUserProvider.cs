using System.Security.Claims;

namespace Magic.Service.Provider;

public interface IUserProvider
{
    Guid? GetUserId();
    Guid? GetUserId(ClaimsPrincipal claimsPrincipal);
    Auth GetAuth();
}