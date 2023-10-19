using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.DTOs.Requests.Modules.Auth
{
    public  class EditRoleModel
    {

        public string Id { get; set; }

        public string NameEN { get; set; }

        public string NameAR { get; set; }

        public string DescriptionEN { get; set; }

        public string DescriptionAR { get; set; }

        public string ParentRoleID { get; set; }

    }


}
