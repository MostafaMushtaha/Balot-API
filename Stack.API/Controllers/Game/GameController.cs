using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
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

        

    }
}
