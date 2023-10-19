using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DatabaseEntities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.GroupMedia
{
    public class Media : BaseEntity
    {
        public string ID { get; set; }

        public long GroupID { get; set; }
        public long CreatorID { get; set; }
        public string? ImageURL { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }

        [ForeignKey("CreatorID")]
        public virtual Group_Member Creator { get; set; }
    }
}
