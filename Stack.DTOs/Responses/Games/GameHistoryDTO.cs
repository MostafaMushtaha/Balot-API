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
        public string GroupName { get; set; }
        public int UserTeamScore { get; set; }
        public int OpponentTeamScore { get; set; }
        public GameMember Members { get; set; }
    }

    public class GameMember
    {
        public List<TeamMemberDTO> FirstTeamMember { get; set; }
        public List<TeamMemberDTO> SecondTeamMember { get; set; }
    }

    public class TeamMemberDTO
    {
        public string UserID { get; set; }
        public string Fullname { get; set; }
    }
}
