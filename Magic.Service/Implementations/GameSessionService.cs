using Magic.Common.Models.Request.GameSessionRequest;
using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Magic.Service.Provider;
using Microsoft.EntityFrameworkCore;

namespace Magic.Service;

public class GameSessionService : IGameSessionService
{
    protected readonly DataBaseContext _dbContext;
    protected readonly IUserProvider _userProvider;

    public GameSessionService(DataBaseContext dbContext, IUserProvider userProvider) 
    { 
        _dbContext = dbContext;
        _userProvider = userProvider;
    }

    /// <summary>
    /// Создать игровую сессию
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Create(CreateGameSessionRequest request)
    {
        var userId = _userProvider.GetUserId();
        await _dbContext.GameSessions.AddAsync(new GameSession
        {
            Title = request.Title,
            Description = request.Description,
            MaxUserCount = request.MaxUserCount,
            StartDt = request.StartDt,
            CreatorUserId = userId!.Value,
            CreatedDate = DateTime.UtcNow,
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Удалить игровую сессию
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Delete(DeleteGameSessionRequest request)
    {
        var gameSession = await _dbContext.GameSessions.FindAsync(request.GameSessionId);
        if (gameSession == null)
        {
            throw new ExceptionWithApplicationCode("Игровая сессия не найдена", Domain.Enums.ExceptionApplicationCodeEnum.GameSessionNotFound);
        }
        _dbContext.GameSessions.Remove(gameSession!);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Выполнить вход в игровую сесси для текущего пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ExceptionWithApplicationCode"></exception>
    public async Task<bool> Enter(EnterToGameSessionRequest request)
    {
        var userId = _userProvider.GetUserId();
        var gameSession = await GetGameSessions(request.GameSessionId);

        if (gameSession.CreatorUserId == userId)
        {
            throw new ExceptionWithApplicationCode("Нельзя войти в игровую сессию, если вы ее создатель", Domain.Enums.ExceptionApplicationCodeEnum.CreatorIdGameSessionEqualsUserIdToEnter);
        }

        var user = await _dbContext.User.FindAsync(userId);

        if (gameSession.Users.Contains(user!))
        {
            throw new ExceptionWithApplicationCode("Вы уже находитесь в этой игровой сессии", Domain.Enums.ExceptionApplicationCodeEnum.UserInGameSession);
        }

        gameSession.Users.Add(user!);
        
        _dbContext.Update(gameSession);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Выгнать пользователя из игровой сессии
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ExceptionWithApplicationCode"></exception>
    public async Task<bool> Kick(KickUserForGameSessionRequest request)
    {
        var userId = _userProvider.GetUserId();
        var gameSession = await GetGameSessions(request.GameSessionId);

        if (gameSession.CreatorUserId != userId)
        {
            throw new ExceptionWithApplicationCode("Ошибка доступа. Вы не можете выполнить это действие для данной игровой сессии", Domain.Enums.ExceptionApplicationCodeEnum.AccessError);
        }

        var user = await _dbContext.User.Include(x => x.GameSessions).FirstOrDefaultAsync(x => x.Id == request.UserId);

        if (user == null)
        {
            throw new ExceptionWithApplicationCode("Пользователь не найден", Domain.Enums.ExceptionApplicationCodeEnum.UserNotExist);
        }

        var gameSessionUser = await _dbContext.GameSessionUser
            .FirstOrDefaultAsync(x => x.GameSessionId == gameSession.Id && x.UserId == request.UserId);

        if (gameSessionUser == null)
        {
            throw new ExceptionWithApplicationCode("Пользователь не найден", Domain.Enums.ExceptionApplicationCodeEnum.UserNotExist);
        }

        _dbContext.GameSessionUser.Remove(gameSessionUser!);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Покинуть игровую сессию
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Leave(LeaveGameSessionRequest request)
    {
        var userId = _userProvider.GetUserId();
        var gameSession = await GetGameSessions(request.GameSessionId);


        var gameSessionUser = await _dbContext.GameSessionUser
            .FirstOrDefaultAsync(x => x.GameSessionId == gameSession.Id && x.UserId == userId);

        if (gameSessionUser == null)
        {
            throw new ExceptionWithApplicationCode("Пользователь не найден", Domain.Enums.ExceptionApplicationCodeEnum.UserNotExist);
        }

        _dbContext.GameSessionUser.Remove(gameSessionUser!);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<GameSession> GetGameSessions(Guid gameSessionId)
    {
        var gameSession = await _dbContext.GameSessions
            .Include(x => x.CreatorUser)
            .Include(x => x.Users)
            .Where(x => x.Id == gameSessionId)
            .FirstOrDefaultAsync();

        if (gameSession == null)
        {
            throw new ExceptionWithApplicationCode("Игровая сессия не найдена", Domain.Enums.ExceptionApplicationCodeEnum.GameSessionNotFound);
        }

        return gameSession;
    }

    /// <summary>
    /// Список игровых сессий
    /// </summary>
    /// <returns></returns>
    public async Task<List<GameSessionResponse>> GetAllGameSession()
    {
        var gameSession = await _dbContext.GameSessions
            .Select(x => new GameSessionResponse(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.MaxUserCount,
                    x.CreatorUserId,
                    x.CreatedDate
                ))
            .ToListAsync();

        return gameSession;
    }
    
    public async Task<GameSession?> GetById(Guid gameSessionId)
    {
        return await _dbContext.GameSessions.FirstOrDefaultAsync(g => g.Id == gameSessionId);
    }
}