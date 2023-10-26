using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.ServiceLayer.Methods.Users;
using ILogger = Serilog.ILogger;

namespace Stack.API.Controllers.Users
{
    [Route("api/Friends")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class FriendsController : BaseResultHandlerController<IFriendsService>
    {
        // private ILogger logger;
        public FriendsController(IFriendsService _service)
            : base(_service) { }

        [HttpGet("AddFriend/{friendID}")]
        public async Task<IActionResult> AddFriend(string friendID)
        {
            return await GetResponseHandler(
                async () => await service.AddFriend(friendID)
            );
        }   
        [HttpPost("RemoveFriend")]
        public async Task<IActionResult> RemoveFriend(string friendID)
        {
            return await AddItemResponseHandler(
                async () => await service.RemoveFriend(friendID)
            );
        }
        [HttpGet("GetUserFriends")]
        public async Task<IActionResult> GetUserFriends()
        {
            return await GetResponseHandler(
                async () => await service.GetUserFriends()
            );
        }

    }
}
