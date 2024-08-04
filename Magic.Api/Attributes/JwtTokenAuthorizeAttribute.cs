using Magic.Api.Extensions;
using Magic.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;
using static Magic.Api.Configure.ModelStateFilter;

namespace Magic.Api.Attributes
{
    public class JwtTokenAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private AppConfig _appConfig;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (IsAllowAnonymous(context))
                return;

            var httpContext = context.HttpContext;
            _appConfig = httpContext.RequestServices.GetRequiredService<AppConfig>();

            if (httpContext.TryJwtToken(_appConfig.authOptions.TokenValidationParameters, out var tokenClaimsIdentity))
                httpContext.User = new GenericPrincipal(tokenClaimsIdentity, new string[] { "User" });
            else
                context.Result = new OkObjectResult(ResponseData<string>.Error("Unauthorized", 401));//new UnauthorizedResult();
        }

        public bool IsAllowAnonymous(AuthorizationFilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
