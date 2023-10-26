using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Stack.DTOs.Requests.Modules.Games
{
    public class RoundStatusModel
    {
        public Teams Teams { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }
        public long GameID { get; set; }
        public int? WinningTeam { get; set; }
    }

    public class Teams
    {
        public List<long> FirstTeamMembers { get; set; }
        public List<long> SecondTeamMembers { get; set; }
    }
}
