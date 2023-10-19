using Stack.DTOs;
using Stack.DTOs.Models;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.Entities.DatabaseEntities.User;

namespace Stack.ServiceLayer.Methods.Auth.User
{
    public interface IUsersService
    {
        public Task<ApiResponse<JwtAccessToken>> Login(LoginModel model);
        public Task<ApiResponse<JwtAccessToken>> GoogleLogin(GoogleLogin model);
        public Task<ApiResponse<JwtAccessToken>> AppleLogin(AppleLogin model);
        // public Task<ApiResponse<bool>> CreateRegistrationRequest(VerifyPhoneNumberModel model);
        public Task<ApiResponse<JwtAccessToken>> GenerateUserToken(ApplicationUser user);
        public Task<ApiResponse<JwtAccessToken>> RefreshAccessToken(RefreshTokenModel model);

        #region  Forgot Password
        // public Task<ApiResponse<string>> RequestEmailPasswordReset(RequestEmailPasswordResetModel model);
        // public Task<ApiResponse<string>> RequestPasswordReset(RequestPasswordResetModel model);
        // public Task<ApiResponse<string>> ValidateUserPasswordOTP(ValidateUserPasswordOTPModel model);
        // public Task<ApiResponse<JwtAccessToken>> UpdateUserPassword(UpdateUserPasswordModel model);
        #endregion
    }
}