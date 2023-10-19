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
        public long ID { get; set; }
        public long Total { get; set; }
        public long GroupID { get; set; }

        // public List<long> Scores { get; set; }
        public virtual Game_Member Winner { get; set; }
        // public virtual ICollection<int> Teams { get; set; }
        // public long Rounds { get; set; }
        public ICollection<GameRound> Rounds { get; set; } //int team number
        public virtual ICollection<Game_Member> GameMembers { get; set; }
        
        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }
    }
}
