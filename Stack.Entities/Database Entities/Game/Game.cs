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
    public class Game : BaseEntity
    {
        // public long Total { get; set; }
        public long GroupID { get; set; }
        public int Status { get; set; }
        public ICollection<GameRound> Rounds { get; set; }
        public virtual ICollection<Game_Member> GameMembers { get; set; }
        
        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }
    }
}
