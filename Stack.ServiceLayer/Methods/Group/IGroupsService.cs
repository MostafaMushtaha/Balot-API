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

namespace Stack.ServiceLayer.Methods.Groups
{
    public interface IGroupsService
    {
        public Task<ApiResponse<long>> CreateGroup(GroupCreationModel model);
        public Task<ApiResponse<bool>> AddGroupMembers(List<string> members, long groupID);
        public Task<ApiResponse<bool>> AddMedia(AddGroupMediaModel model);
        public Task<ApiResponse<bool>> EditGroup(GroupEditModel model);
    }
}
