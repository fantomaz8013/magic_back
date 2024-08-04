using System.Text;
using Magic.Api.Controllers.Websockets;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Magic.Api.Extensions;
using Magic.Common;
using Magic.Service.Extensions;
using Magic.DAL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace Magic.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly string _dbConnectionString;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnectionString = _configuration.GetConnectionString("DBConnectionString");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_configuration);
        services.AddCustomDbContext(_dbConnectionString);
        services.AddControllers();
        services.AddCustomApiVersion();
        services.AddCustomService();
        services.AddCustomSwagger();
        services.AddCustomCors();

        #region Auth
        services.AddAuthorization(options =>
        {
            options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
            {
                policy.AddRequirements()
            });
        });

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var opt = new AppConfig(_configuration).authOptions;
                options.TokenValidationParameters = opt.TokenValidationParameters;
                options.Events = new JwtBearerEvents // Jwt-токен в websocket передается через query string
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (!path.StartsWithSegments(websocketsPath))
                            return Task.CompletedTask;
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments(websocketsPath))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        #endregion

        services.AddValidator();
        services.AddSignalR();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        app.UseCustomCors();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCustomSwagger(provider);
        var builder = WebApplication.CreateBuilder();
        app.UseRouting();

        #region Auth

        app.UseAuthentication();
        app.UseAuthorization();

        #endregion

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ChatHub>("/ws", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                }
            );
        });
        app.UseFileServer(new FileServerOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "storage")),
            RequestPath = "/storage",
            EnableDirectoryBrowsing = false
        });
    }

    private const string websocketsPath = "/ws";
}