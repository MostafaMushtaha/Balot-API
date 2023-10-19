using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public class UpdateUserPasswordModel
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
