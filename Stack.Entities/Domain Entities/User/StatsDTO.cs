using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.Entities.DatabaseEntities.User;

namespace Stack.Entities.DomainEntities.Users
{
    public class UserStatsDTO
    {
        public long? Wins { get; set; }
        public long? Loses { get; set; }
        public long? PlayerLevel { get; set; }
    }
}
