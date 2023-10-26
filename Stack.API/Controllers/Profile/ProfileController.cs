using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.ServiceLayer.Methods.Auth.User;
using Stack.ServiceLayer.Methods.UserProfiles;
using ILogger = Serilog.ILogger;

namespace Stack.API.Controllers.Auth
{
    [Route("api/Profile")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class ProfileController : BaseResultHandlerController<IProfileService>
    {
        // private ILogger logger;
        public ProfileController(IProfileService _service)
            : base(_service) { }

        [HttpGet("ViewPersonalProfile")]
        public async Task<IActionResult> ViewPersonalProfile()
        {
            return await GetResponseHandler(async () => await service.ViewPersonalProfile());
        }
    }
}
