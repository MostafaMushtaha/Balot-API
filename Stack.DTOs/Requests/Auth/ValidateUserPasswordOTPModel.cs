using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public class ValidateUserPasswordOTPModel
    {
        public string PhoneNumber { get; set; }

        public string OTP { get; set; }
    }
}
