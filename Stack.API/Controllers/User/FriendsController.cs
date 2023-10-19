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

        [HttpPost("AddFriend")]
        public async Task<IActionResult> AddFriend(string friendID)
        {
            return await AddItemResponseHandler(
                async () => await service.AddFriend(friendID)
            );
        }
        // [HttpGet("GetUserFriends")]
        // public async Task<IActionResult> GetUserFriends()
        // {
        //     return await AddItemResponseHandler(
        //         async () => await service.GetUserFriends()
        //     );
        // }

    }
}
