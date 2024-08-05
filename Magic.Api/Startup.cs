using System.Text;
using Magic.Api.Controllers.Websockets;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Magic.Api.Extensions;
using Magic.Common;
using Magic.Common.Options;
using Magic.Service.Extensions;
using Magic.DAL.Extensions;
using Magic.Service;
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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = opt.Issuer,
                    ValidAudience = opt.Audience,
                    IssuerSigningKey = opt.SymmetricSecurityKey,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        #endregion

        services.AddValidator();
        services.AddSignalR();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
            endpoints.MapHub<GameSessionsHub>("/ws", options =>
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