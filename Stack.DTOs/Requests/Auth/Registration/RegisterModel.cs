using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberSynonym { get; set; }
        public string Password { get; set; }
        public bool IsPotentialCircleCreator { get; set; }
    }


}
