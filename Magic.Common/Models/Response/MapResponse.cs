using Magic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Common.Models.Response
{
    public class MapResponse
    {
        public Guid Id { get; set; }
        public List<List<int>> Tales { get; set; }

        public MapResponse(Map map)
        {
            Id = map.Id;
            Tales = map.Tales;
        }
    }
}
