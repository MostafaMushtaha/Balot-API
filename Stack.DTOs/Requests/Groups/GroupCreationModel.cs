using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Stack.DTOs.Requests.Modules.Groups
{
    public class GroupCreationModel
    {
        public string GroupName { get; set; }
        public List<string>? Members { get; set; }
    }
}
