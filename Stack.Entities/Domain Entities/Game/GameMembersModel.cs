using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.DTOs.Enums.Modules.Groups;
using Stack.Entities.DomainEntities.Groups;

namespace Stack.Entities.DomainEntities.Games
{
    public class GameMembersModel
    {
        public long GroupID { get; set; }
        public string NewGroupName { get; set; }
    }
}
