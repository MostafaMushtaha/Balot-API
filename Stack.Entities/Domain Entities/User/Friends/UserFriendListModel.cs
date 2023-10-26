using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.DTOs.Enums.Modules.Groups;
using Stack.Entities.DomainEntities.Groups;

namespace Stack.Entities.DomainEntities.Users
{
    public class UserFriendListModel
    {
        public string ID { get; set; }
        public string FriendName { get; set; }
        public string ReferenceNumber { get; set; }
    }
}
