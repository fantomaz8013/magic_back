namespace Magic.Api.Configure;

// public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
// {
//     readonly IApiVersionDescriptionProvider provider;
//
//     public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
//         this.provider = provider;
//
//     public void Configure(SwaggerGenOptions options)
//     {
//         #region Version
//         foreach (var description in provider.ApiVersionDescriptions)
//         {
//             options.SwaggerDoc(
//                 description.GroupName,
//                 new OpenApiInfo
//                 {
//                     Title = $"Magic API {description.ApiVersion}",
//                     Version = description.ApiVersion.ToString(),
//                 });
//         }
//         #endregion
//
//         #region Security
//         options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//         {
//             Description = $"Please enter a valid token",
//             Name = "Authorization",
//             In = ParameterLocation.Header,
//             Type = SecuritySchemeType.Http,
//             BearerFormat = "JWT",
//             Scheme = "Bearer"
//         });
//
//         options.AddSecurityRequirement(new OpenApiSecurityRequirement()
//         {
//             {
//                 new OpenApiSecurityScheme
//                 {
//                     Reference = new OpenApiReference
//                     {
//                         Type = ReferenceType.SecurityScheme,
//                         Id = "Bearer"
//                     },
//                     Name = "Bearer",
//                     In = ParameterLocation.Header,
//                 },
//                 Array.Empty<string>()
//             }
//         });
//         #endregion
//
//     }
// }