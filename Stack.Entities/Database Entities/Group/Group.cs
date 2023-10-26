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
    public class Group : BaseEntity
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Group_Member> Members { get; set; }
        public virtual ICollection<Media>? Media { get; set; }
        public virtual ICollection<Game>? Games { get; set; }
    }
}
