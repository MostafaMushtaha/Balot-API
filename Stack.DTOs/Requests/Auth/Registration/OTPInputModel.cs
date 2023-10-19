using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public class OTPInputModel
    {
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
    }


}
