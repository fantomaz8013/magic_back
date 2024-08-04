using Magic.Common;
using Magic.Common.Models.Response;
using Magic.Common.Options;
using Magic.Domain.Entities;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Magic.Service;

public class TokenService : ITokenService
{
    private readonly AuthOptions _authOptions;
    private readonly HttpContext _httpContext;
    public TokenService(AppConfig appConfig, IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _authOptions = appConfig.authOptions;
    }
    public TokenResponse CreateUserToken(User user, DateTime? refreshTokenEnd = null)
    {
        var now = DateTime.UtcNow;
        var Lifetime = 2880;
        var expires = now + TimeSpan.FromMinutes(Lifetime);
        var expiresRefresh = refreshTokenEnd ?? now + TimeSpan.FromMinutes(Lifetime);
        var claimsIdentity = user.GetUserClaims();
        var jwt = new JwtSecurityToken(
            _authOptions.Issuer,
            _authOptions.Audience,
            notBefore: now,
            claims: claimsIdentity.Claims,
            expires: expires,
            signingCredentials:
            new SigningCredentials(_authOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        jwt = new JwtSecurityToken(
            _authOptions.Issuer,
            _authOptions.Audience,
            notBefore: now,
            claims: claimsIdentity.Claims,
            expires: expiresRefresh,
            signingCredentials:
            new SigningCredentials(_authOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        var tr = new TokenResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expires = expires,
            ExpiresRefresh = expiresRefresh,
            Role = "user",
            UserId = user.Id
        };

        return tr;
    }

    public Guid? GetEntityPublicIdOrThrow(IEnumerable<Claim> claims)
    {
        var claimUserId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claimUserId == null)
            return null;
        //_errorService.TokenIncorrect();
        var outerPublicId = Guid.Parse(claimUserId!.Value);
        //_errorService.SetCurrentUserId(outerPublicId);
        return outerPublicId;
    }
    public Guid? GetUserId(ClaimsPrincipal? claimsPrincipal = null)
    {
        var claims = claimsPrincipal?.Claims.ToArray() ?? _httpContext.User.Claims;

        var claimUserId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claimUserId == null)
            return null;
        var outerPublicId = Guid.Parse(claimUserId.Value);
        return outerPublicId;
    }

    public Auth GetAuth()
    {
        return _httpContext.GetAuth();
    }
    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateLifetime = true, // Because there is no expiration in the generated token
            ValidateAudience = true, // Because there is no audiance in the generated token
            ValidateIssuer = true, // Because there is no issuer in the generated token
            ValidIssuer = _authOptions.Issuer,
            ValidAudience = _authOptions.Audience,
            IssuerSigningKey = _authOptions.SymmetricSecurityKey // The same key as the one that generate the token
        };
    }

    public void ValidateRefreshToken(string refreshToken)
    {
        var validationParams = GetValidationParameters();
        try
        {
            SecurityToken validatedToken;
            new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParams, out validatedToken);
        }
        catch (Exception)
        {
        }
    }
}
public static class OuterEntitiesClaimsExtensions
{
    public static ClaimsIdentity GetUserClaims<T>(this T user) where T : User
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString())  };

        return new ClaimsIdentity(claims, "token");
    }

    public static Auth GetAuth(this HttpContext httpContext)
    {
        var userClaims = httpContext.User.Claims;

        AuthTyoe authType = AuthTyoe.Unknown;
        var _authType = httpContext.User.Identity.AuthenticationType;
        if (_authType == null)
            authType = AuthTyoe.Unknown;
        if (_authType == "token")
            authType = AuthTyoe.User;
        if (_authType == "merchant")
            authType = AuthTyoe.Merchant;

        var claimUserId = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        Guid? userId = null;
        if (claimUserId != null)
            if (Guid.TryParse(claimUserId.Value, out var apiKey))
                userId = apiKey;
        var claimApiKey = userClaims.FirstOrDefault(x => x.Type == "apiKey");
        Guid? merchantId = null;
        if (claimApiKey != null)
            if (Guid.TryParse(claimApiKey.Value, out var merchantId2))
                merchantId = merchantId2;

        var claimIdempotenceKey = userClaims.FirstOrDefault(x => x.Type == "idempotenceKey");
        Guid? idempotenceKey = null;
        if (claimIdempotenceKey != null)
            if (Guid.TryParse(claimIdempotenceKey.Value, out var idempotenceKey2))
                idempotenceKey = idempotenceKey2;

        return new Auth(authType, userId, merchantId, idempotenceKey);
    }
} 

public record class Auth(AuthTyoe authType, Guid? userId, Guid? merchantId, Guid? idempotenceKey);
public enum AuthTyoe { Unknown = 0, User = 1, Merchant = 2 }