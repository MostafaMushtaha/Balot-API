using Stack.Entities.DatabaseEntities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.User
{
    public class Friends : BaseEntity
    {
        public string UserID { get; set; }
        public string FriendID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("FriendID")]
        public virtual ApplicationUser Friend { get; set; }
    }
}
