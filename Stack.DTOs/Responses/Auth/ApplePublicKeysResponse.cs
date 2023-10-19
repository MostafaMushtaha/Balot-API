using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Responses.Auth
{
    public class ApplePublicKeysResponse
    {
        public List<ApplePublicKeysKeyResponse> keys { get; set; }

    }

    public class ApplePublicKeysKeyResponse
    {
        public string kty { get; set; }
        public string kid { get; set; }
        public string use { get; set; }
        public string alg { get; set; }
        public string n { get; set; }
        public string e { get; set; }
    }

}
