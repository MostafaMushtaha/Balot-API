using Stack.DTOs;

namespace Stack.ServiceLayer.Methods.System
{
    public interface ISystemServicesService
    {
        public Task<ApiResponse<bool>> InitializeSystem();
    }
}