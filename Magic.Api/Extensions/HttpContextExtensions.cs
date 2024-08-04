using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Magic.Api.Extensions;

public static class HttpContextExtensions
{
    private const string AUTHORIZATION = "Authorization";
    private const string BEARER = "bearer";

    public static bool TryJwtToken(this HttpContext httpContext, TokenValidationParameters validationParameters, out ClaimsIdentity claimsIdentity)
    {
        claimsIdentity = null;
        try
        {
            if (!httpContext.Request.Headers.TryGetValue(AUTHORIZATION, out var authHeader))
                return false;

            var authHeaderValue = authHeader.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var splittedAuthHeaderValue = authHeaderValue.Split(' ', 2);
            if (splittedAuthHeaderValue.First().ToLowerInvariant() != BEARER)
                return false;

            var tokenAsString = splittedAuthHeaderValue.Last();

            JwtSecurityToken token;
            try
            {
                tokenHandler.ValidateToken(tokenAsString, validationParameters, out var validatedToken);
                token = (JwtSecurityToken)validatedToken;
            }
            catch
            {
                return false;
            }

            if (DateTime.UtcNow < token.ValidFrom)
                return false;


            if (DateTime.UtcNow > token.ValidTo)
                return false;

            claimsIdentity = new ClaimsIdentity(token.Claims, "token");
        }
        catch { return false; }
        return true;
    }
}