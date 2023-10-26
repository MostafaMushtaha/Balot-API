using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.ServiceLayer.Methods.Groups;
using ILogger = Serilog.ILogger;

namespace Stack.API.Controllers.Groups
{
    [Route("api/GroupsManagement")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class GroupsManagemenetController
        : BaseResultHandlerController<IGroupsManagemenetService>
    {
        // private ILogger logger;
        public GroupsManagemenetController(IGroupsManagemenetService _service)
            : base(_service) { }

        [HttpPost("RemoveGroupMember/{groupMemberID}")]
        public async Task<IActionResult> RemoveMember(string groupMemberID)
        {
            return await AddItemResponseHandler(
                async () => await service.RemoveMember(groupMemberID)
            );
        }

        [HttpGet("GetUserInitialGroups")]
        public async Task<IActionResult> GetUserInitialGroups()
        {
            return await GetResponseHandler(async () => await service.GetUserInitialGroups());
        }

        [HttpGet("GetUserGroups")]
        public async Task<IActionResult> GetUserGroups()
        {
            return await GetResponseHandler(async () => await service.GetUserGroups());
        }

        [HttpGet("GetUserGroupDetails")]
        public async Task<IActionResult> GetUserGroupDetails(long GroupID)
        {
            return await AddItemResponseHandler(
                async () => await service.GetUserGroupDetails(GroupID)
            );
        }

        [HttpGet("GetSelectedMembers/{groupID}")]
        public async Task<IActionResult> GetSelectedMembers(long groupID)
        {
            return await AddItemResponseHandler(
                async () => await service.GetSelectedMembers(groupID)
            );
        }
    }
}
