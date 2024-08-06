using FluentValidation;
using FluentValidation.AspNetCore;
using Magic.Api.Configure;
using Magic.Common.Models.Request;

namespace Magic.Api.Extensions;

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