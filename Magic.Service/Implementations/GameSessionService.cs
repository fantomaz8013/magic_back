using Magic.Common.Models.Request.GameSessionRequest;
using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
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
    public async Task<GameSessionResponse> Create(CreateGameSessionRequest request)
    {
        var userId = _userProvider.GetUserId();
        var entity = await _dbContext.GameSessions.AddAsync(new GameSession
        {
            Title = request.Title,
            Description = request.Description,
            MaxUserCount = request.MaxUserCount,
            PlannedStartDate = request.StartDt,
            CreatorUserId = userId!.Value,
            CreatedDate = DateTime.UtcNow,
            GameSessionStatus = GameSessionStatusTypeEnum.WaitingForStart
        });

        await _dbContext.SaveChangesAsync();

        return new GameSessionResponse(entity.Entity);
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
            throw new ExceptionWithApplicationCode("Игровая сессия не найдена",
                ExceptionApplicationCodeEnum.GameSessionNotFound);
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
        var gameSession = await GetGameSessionById(request.GameSessionId);
        
        var user = await _dbContext.User.FindAsync(userId);

        if (gameSession.Users.Contains(user!))
        {
            return true;
        }

        if (gameSession.CreatorUserId == userId)
        {
            return true;
        }

        if (gameSession.GameSessionStatus != GameSessionStatusTypeEnum.WaitingForStart)
        {
            throw new ExceptionWithApplicationCode("Нельзя войти в игровую сессию, if game already started",
                ExceptionApplicationCodeEnum.GameStarted);
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
        var gameSession = await GetGameSessionById(request.GameSessionId);

        if (gameSession.CreatorUserId != userId)
        {
            throw new ExceptionWithApplicationCode(
                "Ошибка доступа. Вы не можете выполнить это действие для данной игровой сессии",
                ExceptionApplicationCodeEnum.AccessError);
        }

        var user = await _dbContext.User.Include(x => x.GameSessions).FirstOrDefaultAsync(x => x.Id == request.UserId);

        if (user == null)
        {
            throw new ExceptionWithApplicationCode("Пользователь не найден",
                ExceptionApplicationCodeEnum.UserNotExist);
        }

        var gameSessionUser = await _dbContext.GameSessionUser
            .FirstOrDefaultAsync(x => x.GameSessionId == gameSession.Id && x.UserId == request.UserId);

        if (gameSessionUser == null)
        {
            throw new ExceptionWithApplicationCode("Пользователь не найден",
                ExceptionApplicationCodeEnum.UserNotExist);
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
        var gameSession = await GetGameSessionById(request.GameSessionId);


        var gameSessionUser = await _dbContext.GameSessionUser
            .FirstOrDefaultAsync(x => x.GameSessionId == gameSession.Id && x.UserId == userId);

        if (gameSessionUser == null)
        {
            throw new ExceptionWithApplicationCode("Пользователь не найден",
                ExceptionApplicationCodeEnum.UserNotExist);
        }

        _dbContext.GameSessionUser.Remove(gameSessionUser!);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    private async Task<GameSession> GetGameSessionById(Guid gameSessionId)
    {
        var gameSession = await _dbContext.GameSessions
            .Include(x => x.CreatorUser)
            .Include(x => x.Users)
            .Include(x => x.Map)
            .Where(x => x.Id == gameSessionId)
            .AsTracking()
            .FirstOrDefaultAsync();

        if (gameSession == null)
        {
            throw new ExceptionWithApplicationCode("Игровая сессия не найдена",
                ExceptionApplicationCodeEnum.GameSessionNotFound);
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
            .Include(x => x.Map)
            .Include(x => x.Users)
            .Select(x => new GameSessionResponse(x))
            .ToListAsync();

        return gameSession;
    }

    public async Task<GameSession?> GetById(Guid gameSessionId)
    {
        return await _dbContext.GameSessions
            .Include(g => g.Users)
            .Include(g => g.CreatorUser)
            .Include(g => g.Map)
            .FirstOrDefaultAsync(g => g.Id == gameSessionId);
    }

    public async Task<bool> ChangeGameSessionStatus(Guid gameSessionId, GameSessionStatusTypeEnum status)
    {
        var gameSession = await GetGameSessionById(gameSessionId);

        if (gameSession.GameSessionStatus > status)
        {
            return false;
        }

        gameSession.GameSessionStatus = status;

        _dbContext.Update(gameSession);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<GameSessionResponse> SetMap(Guid gameSessionId, Guid? MapId)
    {
        var gameSession = await GetGameSessionById(gameSessionId);
        var userId = _userProvider.GetUserId();

        if (userId != gameSession.CreatorUserId) {
            throw new ExceptionWithApplicationCode("Нет доступа",
                ExceptionApplicationCodeEnum.AccessError);
        }

        gameSession.MapId = MapId;
        _dbContext.Update(gameSession);
        await _dbContext.SaveChangesAsync();

        gameSession = await GetGameSessionById(gameSessionId);

        return new GameSessionResponse(gameSession);
    }
}