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
        var result = new ApplyAbilityResponse { IsResult = true };

        var caster = await _gameSessionCharacterService.GetGameSessionCharacter(casterGameSessionCharacterId);
               
        switch (ability.TargetType)
        {
            case CharacterAbilityTargetTypeEnum.Target:
                var target = await _dbContext.GameSessionCharacters
                    .Where(c => c.GameSessionId == caster.GameSessionId && c.PositionX.HasValue && c.PositionY.HasValue && c.PositionX.Value == x && c.PositionY.Value == y)
                    .FirstOrDefaultAsync();
                result = await ApplyTargetAbility(ability, caster, target);
                break;
        }

        return result;
    }

    private async Task<ApplyAbilityResponse> ApplyAreaAbility(CharacterAbility ability, GameSessionCharacter caster, int x, int y)
    {
        var result = new ApplyAbilityResponse { IsResult = true };

        if (await _gameSessionCharacterTurnInfoService.IsCoolDownAbility(caster.Id, ability.Id))
        {
            result.IsResult = false;
            result.Message.Add($"Способность на перезарядке");
            return result;
        }

        if (CalculatePathUtil.Distance(caster.PositionX!.Value, caster.PositionY!.Value, x, y) > ability.Distance)
        {
            result.IsResult = false;
            result.Message.Add($"Цель слишком далеко");
            return result;
        }



        return result;
    }

    private async Task<ApplyAbilityResponse> ApplyTargetAbility(CharacterAbility ability, GameSessionCharacter caster, GameSessionCharacter? target)
    {
        var result = new ApplyAbilityResponse { IsResult = true };

        if (await _gameSessionCharacterTurnInfoService.IsCoolDownAbility(caster.Id, ability.Id))
        {
            result.IsResult = false;
            result.Message.Add($"Способность на перезарядке");
            return result;
        }

        if (target == null)
        {
            result.IsResult = false;
            result.Message.Add($"Тут никого нет, ты чо еблан ?");
            return result;
        }

        //Проверка, находится ли таргет в нужной дистанции от кастующего
        if (CalculatePathUtil.Distance(caster.PositionX!.Value, caster.PositionY!.Value, target.PositionX!.Value, target.PositionY!.Value) > ability.Distance)
        {
            result.IsResult = false;
            result.Message.Add($"Цель слишком далеко");
            return result;
        }

        var rollResult = DiceUtil.RollDice(CubeTypeEnum.D20);

        //Проверка на попадание ( с модификатором главной характеристики класса )
        var modificator = await _gameSessionCharacterService.GetRollModificator(caster.Id, caster.CharacterClass.CharacterCharacteristicId);
        if (rollResult + modificator < target.Armor)
        {
            result.IsResult = false;
            result.Message.Add($"Промазал уебище, старайся лучше");
            return result;
        }

        result.Message.Add(
            $"{caster.Name} попал по {target.Name} (КБ:{target.Armor}) " +
            $"способностью {ability.Title} " +
            $"выкинув {rollResult} к20 + " +
            $"модификатор {caster.CharacterClass.CharacterCharacteristic.Title} {modificator}");

        var rollCount = 0;

        for (int i = 0; i < ability.CubeCount!.Value; i ++)
        {
            rollCount += DiceUtil.RollDice(ability.CubeType!.Value);
        }

        switch(ability.Type)
        {
            case CharacterAbilityTypeEnum.Attack:
                await _gameSessionCharacterService.Damage(target.Id, rollCount);
                result.Message.Add($"Персонаж {target.Name} получил урон в размере: {rollCount}");
                break;
            case CharacterAbilityTypeEnum.Healing:
                await _gameSessionCharacterService.Heal(target.Id, rollCount);
                result.Message.Add($"Персонаж {target.Name} получил лечение в размере: {rollCount}");
                break;
            case CharacterAbilityTypeEnum.Protection:
                await _gameSessionCharacterService.Shield(target.Id, rollCount);
                result.Message.Add($"Персонаж {target.Name} получил защиту в размере: {rollCount}");
                break;
        }

        await _gameSessionCharacterTurnInfoService.UpdateAbilityTurnInfo(caster.Id, ability);

        return result;
    }

    private async Task<CharacterAbility> GetAbility(int characterAbilityId)
    {
        var ability = await _dbContext.CharacterAbilities
            .FirstOrDefaultAsync(x => x.Id == characterAbilityId);

        if (ability == null)
        {
            throw new ExceptionWithApplicationCode("Способность персонажа не найдена",
             ExceptionApplicationCodeEnum.AbilityNotExist);
        }

        return ability;
    }
}