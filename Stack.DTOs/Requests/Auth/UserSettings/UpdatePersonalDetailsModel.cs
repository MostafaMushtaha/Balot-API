using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.DTOs.Requests.Modules.Settings
{
    public class UpdatePersonalDetailsModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Biography { get; set; }
    }


}
