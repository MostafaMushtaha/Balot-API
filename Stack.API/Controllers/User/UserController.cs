using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.Loki;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.ServiceLayer.Methods.Auth.User;
using ILogger = Serilog.ILogger;

namespace Stack.API.Controllers.Auth
{
    [Route("api/Users")]
    [ApiController]
    [Authorize] // Require Authorization to access API endpoints .
    public class UserController : BaseResultHandlerController<IUsersService>
    {
        // private ILogger logger;
        public UserController(IUsersService _service)
            : base(_service) { }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Log.Logger = new LoggerConfiguration()
            // .WriteTo.LokiHttp("http://192.168.8.123:3100")
            // .CreateLogger();

            // Log.Information("Hello, logs!");

            // Log.CloseAndFlush();


            return await AddItemResponseHandler(async () => await service.Login(model));
        }

        [AllowAnonymous]
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(GoogleLogin model)
        {
            return await AddItemResponseHandler(async () => await service.GoogleLogin(model));
        }

        [AllowAnonymous]
        [HttpPost("AppleLogin")]
        public async Task<IActionResult> AppleLogin(AppleLogin model)
        {
            return await AddItemResponseHandler(async () => await service.AppleLogin(model));
        }

        [HttpPost("RefreshAccessToken")]
        public async Task<IActionResult> RefreshAccessToken(RefreshTokenModel model)
        {
            return await AddItemResponseHandler(
                async () => await service.RefreshAccessToken(model)
            );
        }

        #region  Forgot Password

        // [AllowAnonymous]
        // [HttpPost("RequestEmailPasswordReset")]
        // public async Task<IActionResult> RequestEmailPasswordReset(RequestEmailPasswordResetModel model)
        // {
        //     return await AddItemResponseHandler(async () => await service.RequestEmailPasswordReset(model));
        // }


        // [AllowAnonymous]
        // [HttpPost("UpdateUserPassword")]
        // public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordModel model)
        // {
        //     return await AddItemResponseHandler(async () => await service.UpdateUserPassword(model));
        // }

        #endregion
    }
}
