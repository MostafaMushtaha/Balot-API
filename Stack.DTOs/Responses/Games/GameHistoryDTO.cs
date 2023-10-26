using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Responses.Game
{
    public class GameHistoryDTO
    {
        public long GameId { get; set; }
        public DateTime DatePlayed { get; set; }
        public int UserTeamScore { get; set; }
        public int OpponentTeamScore { get; set; }
        public List<string> GameMemberNames { get; set; }
    }
}
