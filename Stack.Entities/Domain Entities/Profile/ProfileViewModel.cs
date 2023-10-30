using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Modules.Profile
{
    public class ProfileViewModel
    {
        public string UserID { get; set; }
        public string? ReferenceNumber { get; set; }
        public string FullName { get; set; }
        public StatsViewModel Stats { get; set; }
    }

    public class StatsViewModel
    {
        public long Wins { get; set; }
        public long Loses { get; set; }
        public long PlayerLevel { get; set; }
    }
}
