using Magic.Api.Controllers.Websockets;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Magic.Api.Extensions;
using Magic.Service.Extensions;
using Magic.DAL.Extensions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.SignalR;

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
            .AddAuthentication()
            .AddJwtBearer(options => { });

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
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseFileServer(new FileServerOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "storage")),
            RequestPath = "/storage",
            EnableDirectoryBrowsing = false
        });

        #region Auth

        app.UseAuthentication();
        app.UseAuthorization();

        #endregion
        
        ConfigureWebsockets(app);
    }

    private const string websocketsPath = "/ws";

    private static void ConfigureWebsockets(IApplicationBuilder app)
    {
        app.Map(
            new PathString(websocketsPath), // Map применяет middleware только при обработке запросов по указанному префиксу пути
            a =>
            {
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseWebSockets();
                app.UseRouting(); // Включает обработку UseEndpoints
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ChatHub>(websocketsPath,
                        configureOptions =>
                        {
                            configureOptions.Transports =
                                HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                        });
                });
            });
    }
}