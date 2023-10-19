using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.UserProfile
{
    public class ProfileSettings : BaseEntity
    {

        public long ProfileID { get; set; }

        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

    }

}
