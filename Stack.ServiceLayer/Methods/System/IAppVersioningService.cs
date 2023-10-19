using Stack.DTOs;
using Stack.DTOs.Responses.System;

namespace Stack.ServiceLayer.Methods.System
{
    public interface IAppVersioningService
    {
        public Task<ApiResponse<AppVersionModel>> VerifyAppVersion();
    }
}