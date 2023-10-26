using Stack.Entities.DatabaseEntities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.UserProfile
{
    public class Profile : BaseEntity
    {
        public string UserID { get; set; }
        public string Thumbnail { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        public virtual ProfileSettings ProfileSettings { get; set; }
    }
}
