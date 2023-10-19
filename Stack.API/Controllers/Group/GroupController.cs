using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.Entities.DomainEntities.Groups;
using Stack.ServiceLayer.Methods.Groups;
using ILogger = Serilog.ILogger;

namespace Stack.API.Controllers.Groups
{
    [Route("api/Groups")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class GroupController : BaseResultHandlerController<IGroupsService>
    {
        // private ILogger logger;
        public GroupController(IGroupsService _service)
            : base(_service) { }

        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup(GroupCreationModel model)
        {
            return await AddItemResponseHandler(async () => await service.CreateGroup(model));
        }

        [HttpPost("AddGroupMembers")]
        public async Task<IActionResult> AddGroupMembers(List<string> members, long groupID)
        {
            return await AddItemResponseHandler(
                async () => await service.AddGroupMembers(members, groupID)
            );
        }

        [HttpPost("EditGroup")]
        public async Task<IActionResult> EditGroup(GroupEditModel model)
        {
            return await AddItemResponseHandler(async () => await service.EditGroup(model));
        }
    }
}
