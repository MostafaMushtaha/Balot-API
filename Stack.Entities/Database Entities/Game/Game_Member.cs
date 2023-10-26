using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DatabaseEntities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.Games
{
    public class Game_Member : BaseEntity
    {
        public long GroupMemberID { get; set; }
        public long GameID { get; set; }
        public int Team { get; set; }
        public bool IsWinner { get; set; }

        [ForeignKey("GroupMemberID")]
        public virtual Group_Member GroupMember { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
    }
}
