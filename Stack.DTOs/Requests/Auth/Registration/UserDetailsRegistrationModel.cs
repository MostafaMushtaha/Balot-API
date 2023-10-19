using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public class UserDetailsRegistrationModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? Address { get; set; }

        public string? Country { get; set; }

        public string? State { get; set; }
    }


}
