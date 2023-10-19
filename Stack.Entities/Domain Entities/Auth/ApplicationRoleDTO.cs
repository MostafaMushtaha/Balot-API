
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Auth
{
    public class ApplicationRoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string NameAR { get; set; }

        public string DescriptionEN { get; set; }
        public string DescriptionAR { get; set; }

        public string ParentRoleID { get; set; }

    }

}
