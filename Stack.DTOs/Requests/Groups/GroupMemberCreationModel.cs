using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Requests.Groups
{
    public class GroupMemberCreationModel
    {
        public long? GroupID { get; set; }
        public string userID { get; set; }
    }


}
