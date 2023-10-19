using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Models;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Auth;

namespace Stack.ServiceLayer.Methods.Auth.Registration
{
    public interface IRegistrationservice
    {
        public Task<ApiResponse<JwtAccessToken>> FinalizeRegistration(RegistrationModel model);
        public Task<ApiResponse<string>> Google_VerifyPhoneNumberRegistration(Google_VerifyPhoneNumberModel model);
    }
}