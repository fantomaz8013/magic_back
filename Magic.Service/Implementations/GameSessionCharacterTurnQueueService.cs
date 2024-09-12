using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Magic.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Magic.Service;

public class GameSessionCharacterTurnQueueService : IGameSessionCharacterTurnQueueService
{
    protected readonly DataBaseContext _dbContext;
    protected readonly ICharacterService _characterService;

    public GameSessionCharacterTurnQueueService(DataBaseContext dbContext, ICharacterService characterService)
    {
        _dbContext = dbContext;
        _characterService = characterService;
    }

    public async Task<GameSessionCharacterTurnQueue> InitTurnQueue(Guid gameSessionId)
    {
        var existed = await GetTurnQueue(gameSessionId);
        if (existed is not null)
            return existed;

        var characters = await _characterService.GetGameSessionCharacters(gameSessionId);
        var initiatives = characters
            .Select(c => new
            {
                Initiative = c.Initiative + DiceUtil.RollDice(CubeTypeEnum.D20),
                CharacterId = c.Id,
            })
            .ToList();

        var entity = await _dbContext.GameSessionCharacterTurnQueues.AddAsync(new GameSessionCharacterTurnQueue
        {
            CurrentIndex = 0,
            GameSessionId = gameSessionId,
            GameSessionCharacterIds = initiatives
                .OrderByDescending(c => c.Initiative)
                .Select(c => c.CharacterId)
                .ToList()
        });

        return entity.Entity;
    }

    public async Task EndTurnQueue(Guid gameSessionId)
    {
        var queue = await GetTurnQueue(gameSessionId);
        if (queue is null) return;

        _dbContext.GameSessionCharacterTurnQueues.Remove(queue);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> NextTurnQueue(Guid gameSessionId)
    {
        var queue = await GetTurnQueue(gameSessionId);
        if (queue is null) return -1;

        queue.CurrentIndex++;
        await _dbContext.SaveChangesAsync();

        return queue.CurrentIndex;
    }

    public async Task<GameSessionCharacterTurnQueue?> GetTurnQueue(Guid gameSessionId)
    {
        return await _dbContext.GameSessionCharacterTurnQueues
            .FirstOrDefaultAsync(q => q.GameSessionId == gameSessionId);
    }
}