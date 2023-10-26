using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.Entities.DatabaseEntities.User;

namespace Stack.Entities.DomainEntities.Groups
{
    public class Group_MemberDTO
    {    
        public long ID { get; set; }
        public string FullName { get; set; }
        public string UserID { get; set; }
        public long GroupID { get; set; }
        public string ReferenceNumber { get; set; }
        public string Title { get; set; }
        public bool IsOwner { get; set; }
        public bool IsSelected { get; set; }
        public Stats Stats { get; set; }
    }

}
