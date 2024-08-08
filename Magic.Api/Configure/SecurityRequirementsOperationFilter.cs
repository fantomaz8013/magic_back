namespace Magic.Api.Configure;

// public class SecurityRequirementsOperationFilter : IOperationFilter
// {
//     public void Apply(OpenApiOperation operation, OperationFilterContext context)
//     {
//         if (context != null && operation != null)
//         {
//             // AuthenticationSchemes map to scopes
//             // for class level authentication schemes
//             var requiredScopes = context.MethodInfo.DeclaringType
//                 .GetCustomAttributes(true)
//                 .OfType<AuthorizeAttribute>()
//                 .Select(attr => attr.AuthenticationSchemes)
//                 .Distinct().ToList();
//
//             //  for method level authentication scheme
//             var requiredScopes2 = context.MethodInfo
//                 .GetCustomAttributes(true)
//                 .OfType<AuthorizeAttribute>()
//                 .Select(attr => attr.AuthenticationSchemes)
//                 .Distinct().ToList();
//
//             var requireAuth = false;
//             var id = "";
//
//             if (requiredScopes.Contains("Bearer") || requiredScopes2.Contains("Bearer"))
//             {
//                 requireAuth = true;
//                 id = "bearerAuth";
//             }
//             else if (requiredScopes.Contains("ApiKey") || requiredScopes2.Contains("ApiKey"))
//             {
//                 requireAuth = true;
//                 id = "apiKeyAuth";
//             }
//             else if (requiredScopes.Contains("Basic") || requiredScopes2.Contains("Basic"))
//             {
//                 requireAuth = true;
//                 id = "basicAuth";
//             }
//             if (context.ApiDescription.GroupName == "ForFront")
//             {
//                 requireAuth = true;
//                 id = "ProjectKey";
//             } else
//             {
//                 requireAuth = true;
//                 id = "Bearer";
//             }
//
//             if (requireAuth && !string.IsNullOrEmpty(id))
//             {
//                 operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
//
//                 if (id == "ProjectKey")
//                     operation.Security = new List<OpenApiSecurityRequirement>
//                     {
//                         new OpenApiSecurityRequirement
//                         {
//                             {
//                                 new OpenApiSecurityScheme
//                                 {
//                                     Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
//                                 },
//                                 new[] { "DemoSwaggerDifferentAuthScheme" }
//                             }
//                         },
//                         new OpenApiSecurityRequirement
//                         {
//                             {
//                                 new OpenApiSecurityScheme
//                                 {
//                                     Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "SignKey" }
//                                 },
//                                 new[] { "DemoSwaggerDifferentAuthScheme" }
//                             }
//                         }
//                     };
//                 else
//                     operation.Security = new List<OpenApiSecurityRequirement>
//                     {
//                         new OpenApiSecurityRequirement
//                         {
//                             {
//                                 new OpenApiSecurityScheme
//                                 {
//                                     Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
//                                 },
//                                 new[] { "DemoSwaggerDifferentAuthScheme" }
//                             }
//                         }
//                     };
//             }
//         }
//     }
// }