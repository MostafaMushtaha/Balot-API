using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stack.DTOs;
using Stack.DTOs.Requests.UserProfile;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Modules.Profile;

namespace Stack.ServiceLayer.Methods.UserProfiles
{
    public interface IProfileService
    {
        public Task<ApiResponse<ProfileViewModel>> ViewPersonalProfile();
        public Task<ApiResponse<string>> UpdateProfileImage(UpdateProfileImageModel model);

    }
}