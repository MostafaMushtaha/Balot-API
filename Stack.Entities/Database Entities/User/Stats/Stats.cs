using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DatabaseEntities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DatabaseEntities.User
{
    public class Stats : BaseEntity
    {
        public long GroupMemberID { get; set; }
        public long Wins { get; set; }
        public long Loses { get; set; }
        public long TotalGames { get; set; }
        public long GroupMemberLevel { get; set; }
        public long WinningStreak { get; set; }

        [ForeignKey("GroupMemberID")]
        public virtual Group_Member GroupMember { get; set; }
    }
}
