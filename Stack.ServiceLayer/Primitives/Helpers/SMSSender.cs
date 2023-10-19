
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
    public class SMSSender : ISMSSender
    {


        public SMSSender()
        {
        }

        public async Task<ApiResponse<string>> SendSMS(string message, string PhoneNumber)
        {
            ApiResponse<string> result = new ApiResponse<string>();

            VLSMSModel smsModel = new VLSMSModel
            {
                UserName = "",
                Password = "",
                SMSText = message,
                SMSLang = "e",
                SMSSender = "",
                SMSReceiver = PhoneNumber,
                SMSID = Guid.NewGuid().ToString()
                //Validity = "4"
            };

            Uri smsUri = new Uri("https://smsvas.vlserv.com/VLSMSPlatformResellerAPI/NewSendingAPI/api/SMSSender/SendSMS");

            string smsJson = JsonConvert.SerializeObject(smsModel);

            var client = new HttpClient();

            var response = await client.PostAsync(smsUri, new StringContent(smsJson, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                result.Succeeded = false;
                return result;
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject(responseBody);

                if (resp != null)
                {
                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    result.Succeeded = false;
                    return result;
                }

            }
        }

    }

}
