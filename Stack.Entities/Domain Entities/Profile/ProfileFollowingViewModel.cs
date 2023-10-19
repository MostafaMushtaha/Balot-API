
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Modules.Profile
{
    public class ProfileFollowingViewModel
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string? Thumbnail { get; set; }

    }

}
