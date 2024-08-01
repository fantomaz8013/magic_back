using Magic.Api.Extensions;
using Magic.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;

namespace Magic.Api.Attributes
{
    public class JwtTokenAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private AppConfig _appConfig;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            _appConfig = httpContext.RequestServices.GetRequiredService<AppConfig>();

            if (httpContext.TryJwtToken(_appConfig.authOptions.TokenValidationParameters, out var tokenClaimsIdentity))
                httpContext.User = new GenericPrincipal(tokenClaimsIdentity, new string[] { "User" });
            else
                context.Result = new UnauthorizedResult();
        }
    }
}
