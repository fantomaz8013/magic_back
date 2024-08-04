namespace Magic.Api.Extensions;

public static class ApiVersionDIExtensions
{
    public static void AddCustomApiVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(opt => {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(opt => {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });
    }
}