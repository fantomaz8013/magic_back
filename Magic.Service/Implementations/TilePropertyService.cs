using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Service.Implementations;

public class TilePropertyService : ITilePropertyService
{
    protected readonly DataBaseContext _dbContext;

    public TilePropertyService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<TileProperty>> GetTileProperties()
    {
        return  await _dbContext.TileProperties
            .ToListAsync();
    }

    public async Task<TileProperty> GetTileProperty(int id)
    {
        var tileProperty = await _dbContext.TileProperties
            .Include(x => x.TilePropertyIfDestroyed)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tileProperty == null)
        {
            throw new ExceptionWithApplicationCode("Параметры тайла не найдены",
                Domain.Enums.ExceptionApplicationCodeEnum.TilePropertyNotExist);
        }

        return tileProperty;
    }
}