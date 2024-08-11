using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Magic.Service.Implementations;

public class GameSessionCharacterService : IGameSessionCharacterService
{
    protected readonly DataBaseContext _dbContext;

    public GameSessionCharacterService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Change(
        GameSessionCharacter gameSessionCharacter,
        Dictionary<string, string> changedCharacterFields
    )
    {
        #region Fields

        var fillableFields =
            new Dictionary<string, Action<string, GameSessionCharacter>>(StringComparer.InvariantCultureIgnoreCase)
            {
                {
                    nameof(GameSessionCharacter.Armor),
                    (value, gameSessionCharacter) => { gameSessionCharacter.Armor = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.Characteristics),
                    (value, gameSessionCharacter) =>
                    {
                        var newCharacteristics = JsonSerializer.Deserialize<Dictionary<int, int>>(value);
                        if (newCharacteristics.Count < 6)
                            throw new ArgumentException("Incorrect number of characteristics");

                        gameSessionCharacter.Characteristics = newCharacteristics;
                    }
                },
                {
                    nameof(GameSessionCharacter.CharacterClassId),
                    (value, gameSessionCharacter) => { gameSessionCharacter.CharacterClassId = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.Description),
                    (value, gameSessionCharacter) => { gameSessionCharacter.Description = value; }
                },
                {
                    nameof(GameSessionCharacter.Name),
                    (value, gameSessionCharacter) => { gameSessionCharacter.Name = value; }
                },
                {
                    nameof(GameSessionCharacter.Speed),
                    (value, gameSessionCharacter) => { gameSessionCharacter.Speed = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.AbilitieIds),
                    (value, gameSessionCharacter) =>
                    {
                        gameSessionCharacter.AbilitieIds = JsonSerializer.Deserialize<int[]>(value);
                    }
                },
                {
                    nameof(GameSessionCharacter.CharacterRaceId),
                    (value, gameSessionCharacter) => { gameSessionCharacter.CharacterRaceId = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.Initiative),
                    (value, gameSessionCharacter) => { gameSessionCharacter.Initiative = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.CurrentShield),
                    (value, gameSessionCharacter) => { gameSessionCharacter.CurrentShield = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.PositionX),
                    (value, gameSessionCharacter) => { gameSessionCharacter.PositionX = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.PositionY),
                    (value, gameSessionCharacter) => { gameSessionCharacter.PositionY = int.Parse(value); }
                },
                {
                    nameof(GameSessionCharacter.AvatarUrL),
                    (value, gameSessionCharacter) => { gameSessionCharacter.AvatarUrL = value; }
                },
                {
                    nameof(GameSessionCharacter.CurrentHP),
                    (value, gameSessionCharacter) =>
                    {
                        gameSessionCharacter.CurrentHP = Math.Min(gameSessionCharacter.MaxHP, int.Parse(value));
                    }
                },
                {
                    nameof(GameSessionCharacter.MaxHP),
                    (value, gameSessionCharacter) => { gameSessionCharacter.MaxHP = int.Parse(value); }
                },
            };

        #endregion

        foreach (var pair in changedCharacterFields.Where(p => fillableFields.ContainsKey(p.Key)))
        {
            fillableFields[pair.Key].Invoke(pair.Value, gameSessionCharacter);
        }

        _dbContext.Update(gameSessionCharacter);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<GameSessionCharacter> Shield(Guid gameSessionCharacterId, int value)
    {
        var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);

        if (!gameSessionCharacter.CurrentShield.HasValue)
        {
            gameSessionCharacter.CurrentShield = 0;
        }

        gameSessionCharacter.CurrentShield += value;
        _dbContext.Update(gameSessionCharacter);
        await _dbContext.SaveChangesAsync();

        return gameSessionCharacter;
    }

    public async Task<GameSessionCharacter> Damage(Guid gameSessionCharacterId, int value)
    {
        var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);

        if (gameSessionCharacter.CurrentShield.HasValue && gameSessionCharacter.CurrentShield <= value)
        {
            value -= gameSessionCharacter.CurrentShield.Value;
            gameSessionCharacter.CurrentShield = null;
        }

        if (gameSessionCharacter.CurrentHP <= value)
        {
            gameSessionCharacter.CurrentHP = 0;
        }
        else
        {
            gameSessionCharacter.CurrentHP -= value;
        }

        _dbContext.Update(gameSessionCharacter);
        await _dbContext.SaveChangesAsync();

        return gameSessionCharacter;
    }

    public async Task<int> GetRollModificator(Guid gameSessionCharacterId, int characterCharacteristicId)
    {
        var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);
        var characterCharacteristicValue = gameSessionCharacter.Characteristics[characterCharacteristicId];

        var result = characterCharacteristicValue switch
        {
            < 2 => -5,
            < 4 and > 1 => -4,
            < 6 and > 3 => -3,
            < 8 and > 5 => -2,
            < 10 and > 7 => -1,
            < 12 and > 9 => 0,
            < 14 and > 11 => 1,
            < 16 and > 13 => 2,
            < 18 and > 15 => 3,
            < 20 and > 17 => 4,
            < 22 and > 19 => 5,
            < 24 and > 21 => 6,
            < 26 and > 23 => 7,
            < 28 and > 25 => 8,
            < 30 and > 27 => 9,
            >= 30 => 10
        };
        return result;
    }

    public async Task<GameSessionCharacter> Heal(Guid gameSessionCharacterId, int value)
    {
        var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);

        gameSessionCharacter.CurrentHP += value;
        _dbContext.Update(gameSessionCharacter);
        await _dbContext.SaveChangesAsync();

        return gameSessionCharacter;
    }

    public async Task<GameSessionCharacter> SetPosition(Guid gameSessionCharacterId, int? x, int? y)
    {
        var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);
        gameSessionCharacter.PositionX = x;
        gameSessionCharacter.PositionY = y;

        _dbContext.Update(gameSessionCharacter);
        await _dbContext.SaveChangesAsync();

        return gameSessionCharacter;
    }

    public async Task<GameSessionCharacter> GetGameSessionCharacter(Guid gameSessionCharacterId)
    {
        var gameSessionCharacter = await _dbContext.GameSessionCharacters
            .Include(x => x.CharacterClass)
            .ThenInclude(x => x.CharacterCharacteristic)
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == gameSessionCharacterId);

        if (gameSessionCharacter == null)
        {
            throw new ExceptionWithApplicationCode("Игровой персонаж не найден",
                Domain.Enums.ExceptionApplicationCodeEnum.GameSessionCharacterNotFound);
        }

        return gameSessionCharacter;
    }
}