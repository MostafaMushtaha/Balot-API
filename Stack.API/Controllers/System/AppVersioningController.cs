using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.ServiceLayer.Methods.System;

namespace Stack.API.Controllers.Spotlight
{
    [Route("api/AppVersioning")]
    [ApiController]
    [Authorize]
    public class AppVersioningController : BaseResultHandlerController<IAppVersioningService>
    {
        public AppVersioningController(IAppVersioningService _service) : base(_service) { }


        [AllowAnonymous]
        [HttpGet("VerifyAppVersion")]
        public async Task<IActionResult> VerifyAppVersion()
        {
            return await GetResponseHandler(async () => await service.VerifyAppVersion());
        }


    }

}
