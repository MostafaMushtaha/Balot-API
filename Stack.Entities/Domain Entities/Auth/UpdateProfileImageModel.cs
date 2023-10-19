
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Auth
{
    public class UpdateProfileImageModel
    {
        public string ID { get; set; }
        public string Image { get; set; }

    }

}
