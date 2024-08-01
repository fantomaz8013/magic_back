using Magic.Api.Configure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Magic.Api.Extensions
{
    public static class SwaggerDIExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(c =>
            {
                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                c.DocInclusionPredicate((name, api) => true);
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
            opt => {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //Hidden schema
                    opt.DefaultModelsExpandDepth(-1);
                    //
                    opt.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"Magic API {description.GroupName.ToUpperInvariant()}");
                }
            });
        }
    }
}
