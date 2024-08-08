using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Service.Implementations
{
    public class MapService : IMapService
    {
        protected readonly DataBaseContext _dbContext;

        public MapService(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<MapResponse>> GetMaps()
        {
            var maps = await _dbContext.Maps
                .Select(x => new MapResponse(x))
                .ToListAsync();
            return maps;
        }
    }
}
