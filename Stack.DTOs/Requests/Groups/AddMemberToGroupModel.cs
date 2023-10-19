using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Stack.DTOs.Requests.Modules.Groups
{
    public class AddMembersToGroupModel
    {
        public long? GroupID { get; set; }
        public string UserID { get; set; }
    }
}
