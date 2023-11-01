using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Requests.Modules.Games;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.DTOs.Responses.Game;
using Stack.Entities.DomainEntities.Games;
using Stack.Entities.DomainEntities.Groups;
using Stack.Entities.DomainEntities.Modules.Profile;

namespace Stack.ServiceLayer.Methods.Games
{
    public interface IGameService
    {
        public Task<ApiResponse<long>> CreateGame(GameCreationModel model);
        public Task<ApiResponse<bool>> CheckRoundStatus(RoundStatusModel model);
        public Task<ApiResponse<bool>> DeleteGame(long gameID);
        public Task<ApiResponse<List<GameHistoryDTO>>> GetRecentGames();
        public Task<ApiResponse<List<GameHistoryDTO>>> GetUserGameHistoryInGroup(long groupID);

    }
}
