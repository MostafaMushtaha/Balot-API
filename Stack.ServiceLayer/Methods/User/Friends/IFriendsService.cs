using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.Entities.DomainEntities.Games;
using Stack.Entities.DomainEntities.Groups;
using Stack.Entities.DomainEntities.Modules.Profile;
using Stack.Entities.DomainEntities.Users;

namespace Stack.ServiceLayer.Methods.Users
{
    public interface IFriendsService
    {
        public Task<ApiResponse<bool>> AddFriend(string userID);
        public Task<ApiResponse<bool>> RemoveFriend(string userID);
        public Task<ApiResponse<List<UserFriendListModel>>> GetUserFriends();
    }
}
