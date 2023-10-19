using Stack.DTOs;

namespace Stack.ServiceLayer.Primitives
{
    public interface IHTTPSender
    {
        public Task<ApiResponse<T>> SendGetRequest<T>(string url);
        public Task<ApiResponse<T>> SendPostRequest<T>(string url, object data);
    }
}