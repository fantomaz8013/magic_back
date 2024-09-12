using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Magic.Service.Provider;
using Magic.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Magic.Service;

public class CharacterAbilityService : ICharacterAbilityService
{
    protected readonly DataBaseContext _dbContext;
    protected readonly IGameSessionCharacterService _gameSessionCharacterService;
    protected readonly IGameSessionCharacterTurnInfoService _gameSessionCharacterTurnInfoService;
    public CharacterAbilityService(DataBaseContext dbContext, IGameSessionCharacterService gameSessionCharacterService, IGameSessionCharacterTurnInfoService gameSessionCharacterTurnInfoService)
    {
        _dbContext = dbContext;
        _gameSessionCharacterService = gameSessionCharacterService;
        _gameSessionCharacterTurnInfoService = gameSessionCharacterTurnInfoService;
    }

    public async Task<ApplyAbilityResponse> ApplyAbility(int characterAbilityId, Guid casterGameSessionCharacterId, int x, int y)
    {
        var ability = await GetAbility(characterAbilityId);
        var result = new ApplyAbilityResponse { IsPossible = true };

        var caster = await _gameSessionCharacterService.GetGameSessionCharacter(casterGameSessionCharacterId);
        var turnInfo = await _gameSessionCharacterTurnInfoService.GetCharacterTurnInfo(casterGameSessionCharacterId);

        if (await _gameSessionCharacterTurnInfoService.IsDisable(casterGameSessionCharacterId))
        {
            result.IsPossible = false;
            result.Messages.Add($"Вы не можете применить способность потому что нейтрализованы. Количество ходов нейтрализации: {turnInfo.SkipStepCount}");
            return result;
        }

        switch (ability.ActionType)
        {
            case CharacterAbilityActionTypeEnum.MainAction:
                if(turnInfo.LeftMainAction == 0) 
                {
                    result.IsPossible = false;
                    result.Messages.Add($"Не хватает очков основного действия");
                    return result;
                }
                break;
            case CharacterAbilityActionTypeEnum.AdditionalAction:
                if(turnInfo.LeftBonusAction == 0) 
                {
                    result.IsPossible = false;
                    result.Messages.Add($"Не хватает очков бонусного действия");
                    return result;
                }
                break;
        }

        switch (ability.TargetType)
        {
            case CharacterAbilityTargetTypeEnum.Target:
            case CharacterAbilityTargetTypeEnum.TargertSelf:
                var target = await _dbContext.GameSessionCharacters
                    .Where(c => c.GameSessionId == caster.GameSessionId && c.PositionX.HasValue && c.PositionY.HasValue && c.PositionX.Value == x && c.PositionY.Value == y)
                    .FirstOrDefaultAsync();
                // здесь внутри проверка на попадания, если не попал тогда IsSuccesful = false
                result = await ApplyTargetAbility(ability, caster, target);
                break;
            case CharacterAbilityTargetTypeEnum.Cone:
                result = await ApplyMultiplePointAbility(ability, caster, x, y);
                break;
            case CharacterAbilityTargetTypeEnum.Area:
                result = await ApplyMultiplePointAbility(ability, caster, x, y);
                break;
        }

        if(result.IsPossible)
        {
            await _gameSessionCharacterTurnInfoService.UpdateAbilityTurnInfo(caster.Id, ability);
            result.Messages.AddRange(await ApplyBuffToTargets(ability, result.TargetIds));
        }

        return result;
    }

    public async Task<List<CharacterAbility>> GetAbilities()
    {
        return await _dbContext.CharacterAbilities.ToListAsync();
    }

    private async Task<List<string>> ApplyBuffToTargets(CharacterAbility ability, List<Guid> targetIds)
    {
        var result = new List<string>();
        if (ability.CharacterBuff == null)
        {
            return result;
        }
        var targets = await _dbContext.GameSessionCharacters
            .Include(x => x.CharacterClass)
            .Where(x => targetIds.Contains(x.Id)).ToListAsync();

        foreach (var target in targets)
        {
            if (ability.TargetCharacterCharacteristicId != null)
            {
                var rollResult = DiceUtil.RollDice(CubeTypeEnum.D20);

                //Проверка на попадание ( с модификатором главной характеристики класса )
                var modificator = await _gameSessionCharacterService.GetRollModificator(target.Id, target.CharacterClass.CharacterCharacteristicId);
                if (rollResult + modificator < 10)
                {
                    result.Add($"Персонаж {target.Name} получает {ability.CharacterBuff.Title}");
                    await _gameSessionCharacterTurnInfoService.UpdateBuffTurnInfo(target.Id, ability);
                }
                else
                {
                    result.Add($"Персонаж {target.Name} прошел проверку и не получает {ability.CharacterBuff.Title}");
                }
            }
        }

        return result;
    }

    private async Task<ApplyAbilityResponse> ApplyMultiplePointAbility(CharacterAbility ability, GameSessionCharacter caster, int x, int y)
    {
        var result = new ApplyAbilityResponse { IsPossible = true };

        if (await _gameSessionCharacterTurnInfoService.IsCoolDownAbility(caster.Id, ability.Id))
        {
            result.IsPossible = false;
            result.Messages.Add($"Способность на перезарядке");
            return result;
        }

        if (CalculatePathUtil.Distance(caster.PositionX!.Value, caster.PositionY!.Value, x, y) > ability.Distance)
        {
            result.IsPossible = false;
            result.Messages.Add($"Цель слишком далеко");
            return result;
        }

        var gameSession = await _dbContext.GameSessions
            .Include(x => x.Map)
            .FirstOrDefaultAsync(x => x.Id == caster.GameSessionId);

        var targetPoints = new List<Point>();

        if (ability.TargetType == CharacterAbilityTargetTypeEnum.Cone)
        {
            targetPoints = CalculateCone.GetPointsInCone(new CalculateConeRequest
            {
                Radius = ability.Radius!.Value,
                UserPosition = new Point(caster.PositionX.Value, caster.PositionY.Value),
                Direction = new Point(x, y)
            }, gameSession!.Map!.Tiles.First().Count, gameSession.Map!.Tiles.Count);
        }

        if (ability.TargetType == CharacterAbilityTargetTypeEnum.Area)
        {
            targetPoints = CalculatePathUtil.CalculatedPointsInArea(x, y, ability.Radius!.Value, gameSession!.Map!.Tiles.First().Count, gameSession.Map!.Tiles.Count);
        }

        var allTargetsGameSessionCharacter = await _dbContext.GameSessionCharacters
            .Where(x => x.PositionX.HasValue && x.PositionY.HasValue && targetPoints.FirstOrDefault(t => t.X == x.PositionX!.Value) != null && targetPoints.Where(t => t.Y == x.PositionY!.Value).FirstOrDefault() != null)
            .ToListAsync();

        var rollCount = 0;

        for (var i = 0; i < ability.CubeCount!.Value; i++)
        {
            rollCount += DiceUtil.RollDice(ability.CubeType!.Value);
        }

        foreach (var target in allTargetsGameSessionCharacter)
        {
            switch (ability.Type)
            {
                case CharacterAbilityTypeEnum.Attack:
                    await _gameSessionCharacterService.Damage(target.Id, rollCount);
                    result.Messages.Add($"Персонаж {target.Name} получил урон в размере: {rollCount}");
                    break;
                case CharacterAbilityTypeEnum.Healing:
                    await _gameSessionCharacterService.Heal(target.Id, rollCount);
                    result.Messages.Add($"Персонаж {target.Name} получил лечение в размере: {rollCount}");
                    break;
                case CharacterAbilityTypeEnum.Protection:
                    await _gameSessionCharacterService.Shield(target.Id, rollCount);
                    result.Messages.Add($"Персонаж {target.Name} получил защиту в размере: {rollCount}");
                    break;
            }
            result.TargetIds.Add(target.Id);
        }

        return result;
    }

    private async Task<ApplyAbilityResponse> ApplyTargetAbility(CharacterAbility ability, GameSessionCharacter caster, GameSessionCharacter? target)
    {
        var result = new ApplyAbilityResponse { IsPossible = true };

        if (await _gameSessionCharacterTurnInfoService.IsCoolDownAbility(caster.Id, ability.Id))
        {
            result.IsPossible = false;
            result.Messages.Add($"Способность на перезарядке");
            return result;
        }

        if (target == null)
        {
            result.IsPossible = false;
            result.Messages.Add($"Тут никого нет, ты чо еблан ?");
            return result;
        }

        if (ability.TargetType == CharacterAbilityTargetTypeEnum.TargertSelf 
            && caster.Id != target.Id)
        {
            result.IsPossible = false;
            result.Messages.Add($"Нельзя применить на другого");
            return result;
        }

        //Проверка, находится ли таргет в нужной дистанции от кастующего
        if (ability.Distance.HasValue && CalculatePathUtil.Distance(caster.PositionX!.Value, caster.PositionY!.Value, target.PositionX!.Value, target.PositionY!.Value) > ability.Distance)
        {
            result.IsPossible = false;
            result.Messages.Add($"Цель слишком далеко");
            return result;
        }

        var rollHit = DiceUtil.RollDice(CubeTypeEnum.D20);

        //Проверка на попадание ( с модификатором главной характеристики класса )
        var modificator = await _gameSessionCharacterService.GetRollModificator(caster.Id, caster.CharacterClass.CharacterCharacteristicId);
        if (rollHit + modificator < target.Armor)
        {
            result.Messages.Add(
                $"{caster.Name} не попал по {target.Name} (КБ:{target.Armor}) " +
                $"способностью {ability.Title} " +
                $"выкинув {rollHit} к20 + " +
                $"модификатор {caster.CharacterClass.CharacterCharacteristic.Title} {modificator}");
            return result;
        }

        result.Messages.Add(
            $"{caster.Name} попал по {target.Name} (КБ:{target.Armor}) " +
            $"способностью {ability.Title} " +
            $"выкинув {rollHit} к20 + " +
            $"модификатор {caster.CharacterClass.CharacterCharacteristic.Title} {modificator}");

        var rollCount = 0;

        for (var i = 0; i < ability.CubeCount!.Value; i ++)
        {
            rollCount += DiceUtil.RollDice(ability.CubeType!.Value);
        }

        switch(ability.Type)
        {
            case CharacterAbilityTypeEnum.Attack:
                await _gameSessionCharacterService.Damage(target.Id, rollCount);
                result.Messages.Add($"Персонаж {target.Name} получил урон в размере: {rollCount}");
                break;
            case CharacterAbilityTypeEnum.Healing:
                await _gameSessionCharacterService.Heal(target.Id, rollCount);
                result.Messages.Add($"Персонаж {target.Name} получил лечение в размере: {rollCount}");
                break;
            case CharacterAbilityTypeEnum.Protection:
                await _gameSessionCharacterService.Shield(target.Id, rollCount);
                result.Messages.Add($"Персонаж {target.Name} получил защиту в размере: {rollCount}");
                break;
        }

        result.TargetIds.Add(target.Id);

        return result;
    }

    private async Task<CharacterAbility> GetAbility(int characterAbilityId)
    {
        var ability = await _dbContext.CharacterAbilities
            .Include(x => x.CharacterBuff)
            .Include(x => x.TargetCharacterCharacteristic)
            .Include(x => x.CasterCharacterCharacteristic)
            .Include(x => x.CharacterClass)
            .FirstOrDefaultAsync(x => x.Id == characterAbilityId);

        if (ability == null)
        {
            throw new ExceptionWithApplicationCode("Способность персонажа не найдена",
             ExceptionApplicationCodeEnum.AbilityNotExist);
        }

        return ability;
    }
}