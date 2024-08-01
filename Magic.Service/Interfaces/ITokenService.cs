using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using System.Security.Claims;

namespace Magic.Service.Interfaces
{
    public interface ITokenService
    {
        TokenResponse CreateUserToken(User user, DateTime? refreshTokenEnd = null);
        Guid? GetEntityPublicIdOrThrow(IEnumerable<Claim> claims);
        Guid? GetUserId(ClaimsPrincipal? claimsPrincipal = null);
        Auth GetAuth();
        void ValidateRefreshToken(string refreshToken);
    }
}
