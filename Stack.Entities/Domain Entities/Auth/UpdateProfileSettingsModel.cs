
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Auth
{
    public class UpdateProfileSettingsModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public int Language { get; set; }

        public int VerificationMethod { get; set; }

    }

}
