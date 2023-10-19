using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Repository.Common
{
    public class HelperFunctions
    {

        public static async Task<string> GetUserID(IHttpContextAccessor _httpContextAccessor)
        {
            return await Task.Run(() =>
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (accessToken != null && accessToken != "")
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(accessToken);
                    var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
                    var userID = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
                    if (userID == null)
                        return null;
                    else
                        return userID;
                }
                else
                    return null;


            });
        }

        // Function to compute the SHA256 Hash of a string . 
        public static async Task<string> ComputeSha256Hash(string rawData)
        {
            return await Task.Run(() =>
            {
                // Create a SHA256   .
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            });
        }

        public static async Task<string> GenerateOTP()
        {
            return await Task.Run(() =>
            {
                Random randomNumber = new Random();
                var chars = "0123456789";
                var generatedOTP = new string(
                    Enumerable.Repeat(chars, 4)
                              .Select(s => s[randomNumber.Next(s.Length)])
                              .ToArray());
                return generatedOTP;
            });
        }

        public static async Task<string> GenerateRandomPassword(PasswordOptions opts = null, object services = null)
        {
            return await Task.Run(() =>
            {
                if (opts == null) opts = new PasswordOptions()
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };

                string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
                };

                Random rand = new Random(Environment.TickCount);
                List<char> chars = new List<char>();

                if (opts.RequireUppercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[0][rand.Next(0, randomChars[0].Length)]);

                if (opts.RequireLowercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[1][rand.Next(0, randomChars[1].Length)]);

                if (opts.RequireDigit)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[2][rand.Next(0, randomChars[2].Length)]);

                if (opts.RequireNonAlphanumeric)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[3][rand.Next(0, randomChars[3].Length)]);

                for (int i = chars.Count; i < opts.RequiredLength
                    || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
                {
                    string rcs = randomChars[rand.Next(0, randomChars.Length)];
                    chars.Insert(rand.Next(0, chars.Count),
                        rcs[rand.Next(0, rcs.Length)]);
                }
                return new string(chars.ToArray());
            });

        }

        public static async Task<DateTime> GetEgyptsCurrentLocalTime()
        {
            return await Task.Run(() =>
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, timeZoneInfo);
                return localTime.DateTime;
            });
        }

        //Generates a 10 digit random number .
        public static string GenerateRandomNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }

        public static async Task<string> base64Encoding(string text) //to base64
        {
            return await Task.Run(() =>
            {
                return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
            });
        }

        public static async Task<string> base64Decoding(string text)//from base64
        {
            return await Task.Run(() =>
            {
                return System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(text));
            });
        }


    }

}
