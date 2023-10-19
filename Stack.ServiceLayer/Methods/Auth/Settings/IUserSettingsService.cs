using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Requests.Modules.Settings;
using Stack.DTOs.Responses.Auth;

namespace Stack.ServiceLayer.Methods.Auth.User
{
    public interface IUserSettingsService
    {
        // public Task<ApiResponse<bool>> UpdatePersonalDetails(UpdatePersonalDetailsModel model);
        public Task<ApiResponse<bool>> UpdatePassword(UpdatePasswordModel model);
        // public Task<ApiResponse<UserProfessionalDetailsModel>> GetUserCharacteristics();
        // public Task<ApiResponse<bool>> UpdateUserCharacteristics(UserCharacteristicsModel model);
        public Task<ApiResponse<bool>> UpdateUserEmail(UpdateUserEmailModel model);

        public Task<ApiResponse<bool>> DeactivateUserAccount();
    }
}