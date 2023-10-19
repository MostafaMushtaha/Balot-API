using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.ServiceLayer.Methods.System;

namespace Stack.API.Controllers.Spotlight
{
    [Route("api/SystemServices")]
    [ApiController]
    [Authorize]
    public class SystemServicesController : BaseResultHandlerController<ISystemServicesService>
    {
        public SystemServicesController(ISystemServicesService _service) : base(_service) { }


        [HttpGet("InitializeSystem")]
        public async Task<IActionResult> InitializeSystem()
        {
            return await GetResponseHandler(async () => await service.InitializeSystem());
        }

    }

}
