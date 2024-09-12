using Magic.Common;
using Magic.Domain.Entities;
using Magic.Service.Implementations;
using Magic.Service.Interfaces;
using Magic.Service.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Magic.Service.Extensions;

public static class ServiceDIExtensions
{
    public static void AddCustomService(this IServiceCollection services)
    {
        services.AddSingleton<AppConfig>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<ILogProvider, LogProvider>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ICharacterService, CharacterService>();
        services.AddScoped<IGameSessionService, GameSessionService>();
        services.AddScoped<IGameSessionMessageService, GameSessionMessageService>();
        services.AddScoped<IGameSessionCharacterService, GameSessionCharacterService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<ITilePropertyService, TilePropertyService>();
        services.AddScoped<IGameSessionCharacterTurnInfoService, GameSessionCharacterTurnInfoService>();
        services.AddScoped<ICharacterAbilityService, CharacterAbilityService>();
        services.AddScoped<IGameSessionCharacterTurnQueueService, GameSessionCharacterTurnQueueService>();
    }
}