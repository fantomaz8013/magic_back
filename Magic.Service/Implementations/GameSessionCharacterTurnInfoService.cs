using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Magic.Service;

public class GameSessionCharacterTurnInfoService : IGameSessionCharacterTurnInfoService
{
    protected readonly DataBaseContext _dbContext;
    protected readonly IGameSessionCharacterService _gameSessionCharacterService;

    public GameSessionCharacterTurnInfoService(DataBaseContext dbContext, IGameSessionCharacterService gameSessionCharacterService)
    {
        _dbContext = dbContext;
        _gameSessionCharacterService = gameSessionCharacterService;
    }

    public async Task<bool> IsCoolDownAbility(Guid gameSessionCharacterId, int abilityId)
    {
        var characterTurnInfo = await GetCharacterTurnInfo(gameSessionCharacterId);

        if (characterTurnInfo.AbilityCoolDowns.FirstOrDefault(x => x.AbilityId == abilityId) != null)
        {
            return true;
        }

        return false;
    }

    public async Task<GameSessionCharacterTurnInfo> UpdateAbilityTurnInfo(Guid gameSessionCharacterId, CharacterAbility ability)
    {
        var characterTurnInfo = await _dbContext.GameSessionCharacterTurnInfos
            .AsTracking()
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId);

        if (ability.CoolDownType != CharacterAbilityCoolDownTypeEnum.None)
        {
            characterTurnInfo!.AbilityCoolDowns.Add(new AbilityCoolDowns
            {
                AbilityId = ability.Id,
                LeftTurns = ability.CoolDownCount,
                CharacterAbilityCoolDownTypeEnum = ability.CoolDownType,
            });
        }

        switch (ability.ActionType)
        {
            case CharacterAbilityActionTypeEnum.MainAction:
                characterTurnInfo.LeftMainAction--;
                break;
            case CharacterAbilityActionTypeEnum.AdditionalAction:
                characterTurnInfo.LeftBonusAction--;
                break;
        }

        _dbContext.Update(characterTurnInfo);
        await _dbContext.SaveChangesAsync();

        return await GetCharacterTurnInfo(gameSessionCharacterId);
    }

    public async Task<GameSessionCharacterTurnInfo> UpdateBuffTurnInfo(Guid gameSessionCharacterId, CharacterAbility ability)
    {
        var characterTurnInfo = await _dbContext.GameSessionCharacterTurnInfos
            .AsTracking()
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId);

        if (ability.CharacterBuffId == null || ability.BuffCount == null)
        {
            return characterTurnInfo!;
        }

        switch(ability.CharacterBuff!.BuffType)
        {
            case Domain.Enums.BuffTypeEnum.Disable:
                characterTurnInfo!.SkipStepCount = ability.BuffCount.Value;
                break;
            case Domain.Enums.BuffTypeEnum.AddMainAction:
                characterTurnInfo!.LeftMainAction += ability.BuffCount.Value;
                break;
        }

        characterTurnInfo!.BuffCoolDowns.Add(new BuffCoolDowns
        {
            BuffId = ability.CharacterBuffId.Value,
            LeftTurns = ability.BuffCount,
        });

        _dbContext.Update(characterTurnInfo);
        await _dbContext.SaveChangesAsync();

        return await GetCharacterTurnInfo(gameSessionCharacterId);
    }

    public async Task<GameSessionCharacterTurnInfo> GetCharacterTurnInfo(Guid gameSessionCharacterId)
    {
        var CharacterTurnInfo = await _dbContext.GameSessionCharacterTurnInfos
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId);

        if (CharacterTurnInfo != null)
        {
            return CharacterTurnInfo;
        }

        var gameSessionCharacter = await _gameSessionCharacterService.GetGameSessionCharacter(gameSessionCharacterId);
        CharacterTurnInfo = await Init(gameSessionCharacter);

        return CharacterTurnInfo;
    }

    private async Task<GameSessionCharacterTurnInfo> Init(GameSessionCharacter gameSessionCharacter)
    {
        var result = await _dbContext.GameSessionCharacterTurnInfos.AddAsync(new GameSessionCharacterTurnInfo
        {
            LeftMainAction = 1,
            LeftBonusAction = 1,
            SkipStepCount = 0,
            LeftStep = gameSessionCharacter.Speed,
            GameSessionCharacterId = gameSessionCharacter.Id,
            AbilityCoolDowns = new List<AbilityCoolDowns>(),
            BuffCoolDowns = new List<BuffCoolDowns>()
        });

        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<GameSessionCharacterTurnInfo> RestoreCharacterTurnInfo(Guid gameSessionCharacterId)
    {
        var characterTurnInfo = await _dbContext.GameSessionCharacterTurnInfos
            .AsTracking()
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId);

        if (characterTurnInfo == null)
        {
            return await GetCharacterTurnInfo(gameSessionCharacterId);
        }

        await ResetCoreInfo(characterTurnInfo, gameSessionCharacterId);

        var abilitiesToDelete = characterTurnInfo.AbilityCoolDowns
            .Where(x => x.CharacterAbilityCoolDownTypeEnum != Domain.Enums.CharacterAbilityCoolDownTypeEnum.OnePerGame)
            .Select(x => x.AbilityId)
            .ToList();

        characterTurnInfo.BuffCoolDowns.Clear();

        characterTurnInfo.AbilityCoolDowns
            .RemoveAll(x => abilitiesToDelete.Contains(x.AbilityId));

        _dbContext.Update(characterTurnInfo);
        await _dbContext.SaveChangesAsync();

        return (await _dbContext.GameSessionCharacterTurnInfos
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId))!;
    }

    private async Task<GameSessionCharacterTurnInfo> ResetCoreInfo(GameSessionCharacterTurnInfo characterTurnInfo, Guid gameSessionCharacterId)
    {
        var gameSessionCharacter = await _gameSessionCharacterService.GetGameSessionCharacter(gameSessionCharacterId);
        characterTurnInfo.LeftStep = gameSessionCharacter.Speed;
        characterTurnInfo.LeftMainAction = 1;
        characterTurnInfo.LeftBonusAction = 1;
        return characterTurnInfo;
    }

    public async Task<GameSessionCharacterTurnInfo> UpdateTurnInfo(Guid gameSessionCharacterId)
    {
        var characterTurnInfo = await _dbContext.GameSessionCharacterTurnInfos
            .AsTracking()
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId);

        if (characterTurnInfo == null)
        {
            throw new ExceptionWithApplicationCode("Информация о ходе не найдена",
                Domain.Enums.ExceptionApplicationCodeEnum.TunfInfoNotExist);
        }

        await ResetCoreInfo(characterTurnInfo, gameSessionCharacterId);

        if (characterTurnInfo.SkipStepCount > 0)
        {
            characterTurnInfo.SkipStepCount--;
        }

        List<int> abilitiesToReset = new();
        List<int> BuffToReset = new();

        foreach (var item in characterTurnInfo.AbilityCoolDowns.Where(x => x.CharacterAbilityCoolDownTypeEnum == Domain.Enums.CharacterAbilityCoolDownTypeEnum.InFight))
        {
            item.LeftTurns--;
            if (item.LeftTurns == 0)
            {
                abilitiesToReset.Add(item.AbilityId);
            }
        }

        foreach (var item in characterTurnInfo.BuffCoolDowns)
        {
            item.LeftTurns--;
            if (item.LeftTurns == 0)
            {
                BuffToReset.Add(item.BuffId);
            }
        }
        

        characterTurnInfo.AbilityCoolDowns
            .RemoveAll(x => abilitiesToReset.Contains(x.AbilityId));
        characterTurnInfo.BuffCoolDowns
            .RemoveAll(x => BuffToReset.Contains(x.BuffId));

        _dbContext.Update(characterTurnInfo);
        await _dbContext.SaveChangesAsync();

        return (await _dbContext.GameSessionCharacterTurnInfos
            .FirstOrDefaultAsync(x => x.GameSessionCharacterId == gameSessionCharacterId))!;
    }

    public async Task<bool> IsDisable(Guid gameSessionCharacterId)
    {
        var turnInfo = await GetCharacterTurnInfo(gameSessionCharacterId);

        if (turnInfo.SkipStepCount > 0)
        {
            return true;
        }

        return false;
    }
}