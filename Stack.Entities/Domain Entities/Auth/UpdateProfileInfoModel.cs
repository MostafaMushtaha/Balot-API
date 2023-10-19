
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Auth
{
    public class UpdateProfileInfoModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string TwitterHandle { get; set; }
        public string FacebookHandle { get; set; }
        public string LinkedInHandle { get; set; }
     
        public string Biography { get; set; }

    }

}
