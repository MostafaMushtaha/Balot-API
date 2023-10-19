using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Responses.Auth
{
    public class UpdateUserPhoneNumberModel
    {
        public string InternationalPhoneNumber { get; set; }
        public string NationalPhoneNumber { get; set; }
        public string DialCode { get; set; }
        public string ISOCode { get; set; }
    }
}
