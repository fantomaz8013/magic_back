using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Magic.Service;

public class CharacterService : ICharacterService
{
    protected readonly DataBaseContext _dbContext;

    public CharacterService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GameSessionCharacter>> ChooseCharacters(Dictionary<Guid, Guid> userIdsToCharacterTemplatesIds, Guid gameSessionId)
    {
        var gameSession = await _dbContext.GameSessions
            .FindAsync(gameSessionId);

        if (gameSession == null)
        {
            throw new ExceptionWithApplicationCode("Игровая сессия не найдена",
               Domain.Enums.ExceptionApplicationCodeEnum.GameSessionNotFound);
        }

        foreach (var item in userIdsToCharacterTemplatesIds)
        {
            var userId = item.Key;
            var characterTemplateId = item.Value;
            var user = await _dbContext.User
                .FindAsync(userId);

            if (user == null)
            {
                throw new ExceptionWithApplicationCode("Пользователь не найден",
                    Domain.Enums.ExceptionApplicationCodeEnum.UserNotExist);
            }

            var characterTemplate = await _dbContext.CharacterTemplates
                .FindAsync(characterTemplateId);

            if (characterTemplate == null)
            {
                throw new ExceptionWithApplicationCode("Шаблон персонажа не найден",
                    Domain.Enums.ExceptionApplicationCodeEnum.CharacterTemplateNotExist);
            }

            await _dbContext.GameSessionCharacters.AddAsync(new GameSessionCharacter
            {
                Name = characterTemplate.Name,
                Description = characterTemplate.Description,
                AvatarUrL = characterTemplate.AvatarUrL,
                CharacterClassId = characterTemplate.CharacterClassId,
                AbilitieIds = characterTemplate.AbilitieIds,
                Armor = characterTemplate.Armor,
                CharacterRaceId = characterTemplate.CharacterRaceId,
                MaxHP = characterTemplate.MaxHP,
                Speed = characterTemplate.Speed,
                Initiative = characterTemplate.Initiative,
                Characteristics = characterTemplate.Characteristics,
                CurrentHP = characterTemplate.MaxHP,
                OwnerId = userId,
                GameSessionId = gameSessionId,
            });

        }

        var gameSessionCharacters = await GetGameSessionCharacters(gameSessionId);

        return gameSessionCharacters;
    }

    public async Task<List<CharacterCharacteristic>> GetCharacterCharacteristics()
    {
        var characteristics = await _dbContext.CharacterCharacteristic.ToListAsync();
        return characteristics;
    }

    public async Task<List<CharacterRace>> GetCharacterRaces()
    {
        var races = await _dbContext.CharacterRaces.ToListAsync();
        return races;
    }

    public async Task<List<CharacterTemplateResponse>> GetCharacterTemplates()
    {
        var templates = await _dbContext
            .CharacterTemplates
            .Include(x => x.CharacterRace)
            .Include(x => x.CharacterClass)
            .ThenInclude(c => c.CharacterCharacteristic)
            .ToListAsync();
        var requiredAbilities = templates
            .SelectMany(t => t.AbilitieIds)
            .Distinct()
            .ToList();
        var abilities = await _dbContext
            .CharacterAbilities
            .Include(a => a.CasterCharacterCharacteristic)
            .Include(a => a.TargetCharacterCharacteristic)
            .Where(a => requiredAbilities.Contains(a.Id))
            .ToDictionaryAsync(ability => ability.Id, ability => ability);

        return templates
            .Select(t =>
                new CharacterTemplateResponse(t, t.AbilitieIds
                    .Select(aId => abilities[aId])
                    .ToArray())
            )
            .ToList();
    }

    public async Task<List<CharacterClass>> GetClasses()
    {
        var classes = await _dbContext.CharacterClasses
            .Include(x => x.CharacterCharacteristic)
            .ToListAsync();
        return classes;
    }

    public async Task<List<CharacterAvatar>> GetDefaultAvatar()
    {
        var avatars = await _dbContext.CharacterAvatars.ToListAsync();
        return avatars;
    }

    public async Task<List<GameSessionCharacter>> GetGameSessionCharacters(Guid gameSessionId)
    {
        var result = await _dbContext.GameSessionCharacters
             .Include(x => x.CharacterClass)
             .Include(x => x.CharacterRace)
             .Where(x => x.GameSessionId == gameSessionId)
             .ToListAsync();

        return result;
    }


}