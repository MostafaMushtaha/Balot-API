using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.Entities.DomainEntities.Auth;
using Stack.ServiceLayer.Methods.Auth.Registration;

namespace Stack.API.Controllers.Auth
{
    [Route("api/Registration")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class RegistrationController : BaseResultHandlerController<IRegistrationservice>
    {
        public RegistrationController(IRegistrationservice _service)
            : base(_service) { }

        [AllowAnonymous]
        [HttpPost("FinalizeRegistration")]
        public async Task<IActionResult> RegisterUser(RegistrationModel model)
        {
            return await GetResponseHandler(async () => await service.FinalizeRegistration(model));
        }

        [AllowAnonymous]
        [HttpPost("Google_VerifyPhoneNumberRegistration")]
        public async Task<IActionResult> Google_VerifyPhoneNumberRegistration(
            Google_VerifyPhoneNumberModel model
        )
        {
            return await AddItemResponseHandler(
                async () => await service.Google_VerifyPhoneNumberRegistration(model)
            );
        }
    }
}
