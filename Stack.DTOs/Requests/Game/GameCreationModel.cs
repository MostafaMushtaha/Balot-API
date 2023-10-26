using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Stack.DTOs.Requests.Modules.Games
{
    public class GameCreationModel
    {
        public List<GameMembers> GameMembers { get; set; }
        public long GroupID { get; set; }
    }

    public class GameMembers
    {
        public long GameMemberID { get; set; }
        public int Team { get; set; }
    }
}
