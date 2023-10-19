using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Settings;
using Stack.DTOs.Responses.Auth;
using Stack.ServiceLayer.Methods.Auth.User;

namespace Stack.API.Controllers.Auth
{
    [Route("api/UserSettings")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints . 
    public class UserSettingsController : BaseResultHandlerController<IUserSettingsService>
    {
        public UserSettingsController(IUserSettingsService _service) : base(_service)
        {

        }


        // [HttpPost("UpdatePersonalDetails")]
        // public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsModel model)
        // {
        //     return await AddItemResponseHandler(async () => await service.UpdatePersonalDetails(model));
        // }


        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            return await AddItemResponseHandler(async () => await service.UpdatePassword(model));
        }

        [HttpPost("UpdateUserEmail")]
        public async Task<IActionResult> UpdateUserEmail(UpdateUserEmailModel model)
        {
            return await AddItemResponseHandler(async () => await service.UpdateUserEmail(model));
        }

        [HttpGet("DeleteUserAccount")]
        public async Task<IActionResult> DeactivateUserAccount()
        {
            return await GetResponseHandler(async () => await service.DeactivateUserAccount());
        }
    }
}
