using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Magic.Common.Options
{
    public class AuthOptions
    {
        public AuthOptions(
       string jwtKey,
       string issuer,
       string audience,
       int lifetime
   )
        {
            JwtKey = jwtKey;
            Issuer = issuer;
            Audience = audience;
            Lifetime = lifetime;
            SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtKey));
            TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = SymmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidAudience = Audience,
                ValidateAudience = true,
                ValidIssuer = Issuer,
                ValidateIssuer = true,
                ValidateLifetime = false
            };
        }
        public string JwtKey { get; }

        public string Issuer { get; }

        public string Audience { get; }

        public int Lifetime { get; }
        public TimeSpan LifetimeSpan => TimeSpan.FromMinutes(Lifetime);

        public SymmetricSecurityKey SymmetricSecurityKey { get; }

        public TokenValidationParameters TokenValidationParameters { get; }       
    }
}
