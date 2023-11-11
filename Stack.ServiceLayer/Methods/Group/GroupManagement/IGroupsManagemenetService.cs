using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Requests.Groups;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.Entities.DatabaseEntities.GroupMedia;
using Stack.Entities.DomainEntities.Groups;
using Stack.Entities.DomainEntities.Modules.Profile;

namespace Stack.ServiceLayer.Methods.Groups
{
    public interface IGroupsManagemenetService
    {
        public Task<ApiResponse<UserGroupsModel>> GetUserInitialGroups();
        public Task<ApiResponse<List<GroupModel>>> GetUserGroups();
        public Task<ApiResponse<UserGroupDetailsModel>> GetUserGroupDetails(long groupID);
        public Task<ApiResponse<bool>> RemoveMember(string groupMemberID);
        public Task<ApiResponse<List<Group_MemberDTO>>> GetSelectedMembers(long groupId);
        public Task<ApiResponse<List<Media>>> GetGroupMediaList(GroupMediaModel model);
    }
}
