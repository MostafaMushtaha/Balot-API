
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DatabaseEntities.Auth
{
    public class ApplicationRole : IdentityRole
    {
       
        public string NameAR { get; set; }

        public string DescriptionEN { get; set; }

        public string DescriptionAR { get; set; }

        public bool HasParent { get; set; }

        public string ParentRoleID { get; set; }

    }

}
