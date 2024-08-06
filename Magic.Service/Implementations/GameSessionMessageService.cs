using System.Linq.Dynamic.Core;
using Magic.Common.Models.Response;
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

    public async Task<ChatGameGameSessionMessageResponse> AddChatMessage(Guid gameSessionId, string message)
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

        var res = await _dbContext.ChatGameSessionMessages
            .Include(m => m.Author)
            .FirstOrDefaultAsync(m => m.Id == rEntry.Entity.Id)!;
        return new ChatGameGameSessionMessageResponse(res);
    }

    public async Task<ServerGameSessionMessageResponse> AddServerMessage(Guid gameSessionId, string message)
    {
        var rEntry = await _dbContext.ServerSessionMessages.AddAsync(new ServerGameSessionMessage
        {
            GameSessionId = gameSessionId,
            Message = message,
            CreatedDate = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();

        var res = await _dbContext.ServerSessionMessages
            .FirstOrDefaultAsync(m => m.Id == rEntry.Entity.Id)!;

        return new ServerGameSessionMessageResponse(res);
    }

    public async Task<DiceGameSessionMessageResponse> AddDiceMessage(Guid gameSessionId, int diceRoll,
        CubeTypeEnum cubeTypeEnum)
    {
        var userId = _userProvider.GetUserId();
        var rEntry = await _dbContext.DiceSessionMessages.AddAsync(new DiceGameSessionMessage
        {
            GameSessionId = gameSessionId,
            CubeTypeEnum = cubeTypeEnum,
            Roll = diceRoll,
            CreatedDate = DateTime.UtcNow,
            AuthorId = userId.Value
        });

        await _dbContext.SaveChangesAsync();

        var res = await _dbContext.DiceSessionMessages
            .Include(m => m.Author)
            .FirstOrDefaultAsync(m => m.Id == rEntry.Entity.Id)!;

        return new DiceGameSessionMessageResponse(res);
    }

    public async Task<List<BaseGameSessionMessageResponse>> GetMessages(Guid gameSessionId)
    {
        var messages = await _dbContext.GameSessionMessages
            .Include(m => (m as ChatGameGameSessionMessage).Author)
            .Include(m => (m as DiceGameSessionMessage).Author)
            .Where(g => g.GameSessionId == gameSessionId)
            .OrderBy(g => g.CreatedDate)
            .ToListAsync();


        var responses = new List<BaseGameSessionMessageResponse>();
        foreach (var message in messages)
        {
            BaseGameSessionMessageResponse response = message switch
            {
                ChatGameGameSessionMessage chatGameGameSessionMessage
                    => new ChatGameGameSessionMessageResponse(chatGameGameSessionMessage),
                ServerGameSessionMessage serverGameSessionMessage
                    => new ServerGameSessionMessageResponse(serverGameSessionMessage),
                DiceGameSessionMessage diceGameSessionMessage
                    => new DiceGameSessionMessageResponse(diceGameSessionMessage),
                _ => throw new Exception($"{message.GetType().FullName} is not supported")
            };
            responses.Add(response);
        }

        return responses;
    }
}