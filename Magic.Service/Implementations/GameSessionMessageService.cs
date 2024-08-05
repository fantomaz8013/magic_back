using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Magic.Service.Interfaces;
using Magic.Service.Provider;
using Microsoft.EntityFrameworkCore;

namespace Magic.Service;

public class GameSessionMessageService : IGameSessionMessageService
{
    protected readonly DataBaseContext _dbContext;
    protected readonly IUserProvider _userProvider;

    public GameSessionMessageService(DataBaseContext dbContext, IUserProvider userProvider)
    {
        _dbContext = dbContext;
        _userProvider = userProvider;
    }

    public async Task<ChatGameGameSessionMessage> AddChatMessage(Guid gameSessionId, string message)
    {
        var userId = _userProvider.GetUserId();

        var rEntry = await _dbContext.ChatGameSessionMessages.AddAsync(new ChatGameGameSessionMessage
        {
            GameSessionId = gameSessionId,
            AuthorId = userId!.Value,
            Message = message,
            CreatedDate = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();

        return (await _dbContext.ChatGameSessionMessages
            .Include(m => m.Author)
            .FirstOrDefaultAsync(m => m.Id == rEntry.Entity.Id))!;
    }

    public async Task<ServerGameSessionMessage> AddServerMessage(Guid gameSessionId, string message)
    {
        var rEntry = await _dbContext.ServerSessionMessages.AddAsync(new ServerGameSessionMessage
        {
            GameSessionId = gameSessionId,
            Message = message,
            CreatedDate = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();

        return (await _dbContext.ServerSessionMessages
            .FirstOrDefaultAsync(m => m.Id == rEntry.Entity.Id))!;
    }

    public async Task<DiceGameSessionMessage> AddDiceMessage(Guid gameSessionId, int diceRoll,
        CubeTypeEnum cubeTypeEnum)
    {
        var rEntry = await _dbContext.DiceSessionMessages.AddAsync(new DiceGameSessionMessage
        {
            GameSessionId = gameSessionId,
            CubeTypeEnum = cubeTypeEnum,
            Roll = diceRoll,
            CreatedDate = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();

        return (await _dbContext.DiceSessionMessages
            .FirstOrDefaultAsync(m => m.Id == rEntry.Entity.Id))!;
    }

    public async Task<List<BaseGameSessionMessage>> GetMessages(Guid gameSessionId)
    {
        return await _dbContext.GameSessionMessages
            .Include(m => (m as ChatGameGameSessionMessage).Author)
            .Where(g => g.GameSessionId == gameSessionId)
            .OrderBy(g => g.CreatedDate)
            .ToListAsync();
    }
}