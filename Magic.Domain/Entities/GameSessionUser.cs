using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain.Entities
{
    public class GameSessionUser : BaseEntity<int>
    {
        public Guid GameSessionId { get; set; }
        public Guid UserId { get; set; }
        
    }
}
