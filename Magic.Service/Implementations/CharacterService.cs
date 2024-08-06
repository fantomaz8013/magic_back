using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
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
}