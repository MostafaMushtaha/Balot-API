using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.ServiceLayer.Methods.User;

namespace Stack.API.Controllers.Social
{
    [Route("api/Devices")]
    [ApiController]
    [Authorize]
    public class UserDevicesController : BaseResultHandlerController<IUserDevicesService>
    {
        public UserDevicesController(IUserDevicesService _service) : base(_service)
        {

        }

        [AllowAnonymous]
        [HttpPost("RegisterDevice")]
        public async Task<IActionResult> RegisterDevice(UserDeviceModel model)
        {
            return await AddItemResponseHandler(async () => await service.RegisterDevice(model));
        }

        [AllowAnonymous]
        [HttpGet("DeactivateDevice")]
        public async Task<IActionResult> DeactivateDevice()
        {
            return await GetResponseHandler(async () => await service.DeactivateDevice());
        }

    }

}
