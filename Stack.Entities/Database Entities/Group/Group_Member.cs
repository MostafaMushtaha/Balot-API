using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.DatabaseEntities.GroupMedia;
using Stack.Entities.DatabaseEntities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.Groups
{
    public class Group_Member : BaseEntity
    {
        public string UserID { get; set; }
        public long GroupID { get; set; }
        public bool IsOwner { get; set; }
        public bool IsSelected { get; set; }
        public int Status { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }
        public virtual Stats Stats { get; set; }
        public virtual ICollection<Game_Member>? GameMembers { get; set; }
        public virtual ICollection<Media>? Media { get; set; }
    }
}
