using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public class Google_VerifyPhoneNumberModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public string PhoneNumberSynonym { get; set; }

        public bool IsPotentialCircleCreator { get; set; }
    }


}
