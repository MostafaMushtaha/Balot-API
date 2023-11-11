using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Stack.DTOs.Requests.Modules.Groups
{
    public class AddGroupMediaModel
    {
        public long GroupID { get; set; }
        public IFormFile? Media { get; set; }
    }
}
