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
    public class GameRound : BaseEntity
    {
        public long GameID { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }
        
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
    }
}
