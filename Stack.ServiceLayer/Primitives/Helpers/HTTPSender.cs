
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Stack.Entities.DomainEntities.Auth;
using Stack.Entities.DomainEntities;
using Newtonsoft.Json;
using System.Text;
using Stack.DTOs;

namespace Stack.ServiceLayer.Primitives
{
    public class HTTPSender : IHTTPSender
    {
        private readonly IHttpClientFactory _clientFactory;

        public HTTPSender(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ApiResponse<T>> SendGetRequest<T>(string url)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(url);

            return await ProcessResponse<T>(response);
        }

        public async Task<ApiResponse<T>> SendPostRequest<T>(string url, object data)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                return await ProcessResponse<T>(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // _logger.LogError(ex, "Error occurred while sending post request.");

                return new ApiResponse<T>
                {
                    Succeeded = false,
                    Errors = new List<string> { "An error occurred while processing the response." }
                };
            }
        }

        private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
        {
            var result = new ApiResponse<T>();

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<T>(responseBody);

                if (responseData != null)
                {
                    result.Succeeded = true;
                    result.Data = responseData;
                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Add("Failed to deserialize the response data.");
                }
            }
            else
            {
                result.Succeeded = false;
                // Add the error content to the Errors list
                result.Errors.Add(responseBody);
            }

            return result;
        }
    }
}
