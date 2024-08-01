using Magic.Common.Models.Request;
using FluentValidation;
using FluentValidation.AspNetCore;
using Magic.Api.Configure;

namespace Magic.Api.Extensions
{
    public static class ValidatorDIExtensions
    {
        public static void AddValidator(this IServiceCollection services)
        {
            services
                .AddMvc(options => { options.Filters.Add(new ModelStateFilter()); })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<TokenRequestValidator>();
                    fv.ImplicitlyValidateChildProperties = true;
                });
            services.AddScoped<IValidator<UserRequest>, UserRequestValidator>();
        }
    }
}
