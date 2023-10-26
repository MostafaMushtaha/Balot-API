using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Requests.Modules.Games;
using Stack.ServiceLayer.Methods.Games;
using ILogger = Serilog.ILogger;

namespace Stack.API.Controllers.Games
{
    [Route("api/Games")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class GameController : BaseResultHandlerController<IGameService>
    {
        // private ILogger logger;
        public GameController(IGameService _service)
            : base(_service) { }

        [HttpPost("CreateGame")]
        public async Task<IActionResult> StartGame(GameCreationModel model)
        {
            return await AddItemResponseHandler(async () => await service.CreateGame(model));
        }
        [HttpPost("CheckRoundStatus")]
        public async Task<IActionResult> CheckRoundStatus(RoundStatusModel model)
        {
            return await AddItemResponseHandler(async () => await service.CheckRoundStatus(model));
        }
        [HttpGet("DeleteGame/{gameID}")]
        public async Task<IActionResult> DeleteGame(long gameID)
        {
            return await GetResponseHandler(async () => await service.DeleteGame(gameID));
        }
        [HttpGet("GetRecentGames")]
        public async Task<IActionResult> GetRecentGames()
        {
            return await GetResponseHandler(async () => await service.GetRecentGames());
        }

    }
}
