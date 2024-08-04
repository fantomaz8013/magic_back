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

    public async Task<List<CharacterClass>> GetClasses()
    {
        var classes = await _dbContext.CharacterClasses
            .Include(x => x.characterCharacteristic)
            .ToListAsync();
        return classes;
    }

    public async Task<List<CharacterAvatar>> GetDefaultAvatar()
    {
        var avatars = await _dbContext.CharacterAvatars.ToListAsync();
        return avatars;
    }

}