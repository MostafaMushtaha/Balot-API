using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.DTOs.Enums.Modules.Groups;
using Stack.Entities.DomainEntities.Groups;

namespace Stack.Entities.DomainEntities.Groups
{
    public class UserGroupsModel
    {
        public long GroupID { get; set; }
        public string Name { get; set; }
    }
}
