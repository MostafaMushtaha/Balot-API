using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.Entities.DomainEntities.Modules.Profile;

namespace Stack.ServiceLayer.Methods.User
{
    public interface IUserDevicesService
    {
        public Task<ApiResponse<bool>> RegisterDevice(UserDeviceModel model);
        public Task<ApiResponse<bool>> DeactivateDevice();
    }
}