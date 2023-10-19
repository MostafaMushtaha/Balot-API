
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Modules.UserProfile
{
    public class ProfileQuickViewModel
    {
        //Unsynced contact 
        public long? ID { get; set; }
        public string? UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Thumbnail { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberSynonym { get; set; }
        public bool IsFollowed { get; set; }
    }

}
