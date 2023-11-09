using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.DTOs.Enums.Modules.Groups;
using Stack.Entities.DatabaseEntities.GroupMedia;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Auth;
using Stack.Entities.DomainEntities.Groups;

namespace Stack.Entities.DomainEntities.Groups
{
    public class UserGroupDetailsModel
    {
        public List<Media>? Media { get; set; }
        public List<Group_MemberDTO>? GroupMembers { get; set; }
    }
}
