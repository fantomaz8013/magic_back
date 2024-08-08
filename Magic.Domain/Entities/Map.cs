using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain.Entities
{
    public class Map : BaseEntity<Guid>
    {
        public List<List<int>> Tales { get; set; }

    }
}
