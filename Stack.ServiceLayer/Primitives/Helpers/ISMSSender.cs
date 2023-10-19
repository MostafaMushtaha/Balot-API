using Stack.DTOs;

namespace Stack.ServiceLayer.Primitives
{
    public interface ISMSSender
    {
        public Task<ApiResponse<string>> SendSMS(string message, string PhoneNumber);
    }
}