
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Modules.UserProfile
{
    public class FilteredContactsSyncingModel
    {
        public List<ProfileQuickViewModel> SyncedProfiles { get; set; }
        public List<ProfileQuickViewModel> UnsyncedProfiles { get; set; }
    }

}
