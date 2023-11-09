using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.DTOs.Models;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.DTOs.Responses.Auth;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Auth.Registration;
using Stack.Entities.Enums.Modules.Auth;
using Stack.Repository.Common;
using Stack.ServiceLayer.Methods.Auth.User;
using Stack.ServiceLayer.Primitives;

// using Serilog;

namespace Stack.ServiceLayer.Methods.Auth
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly ILogger<UsersService> _logger;
        private readonly ISMSSender _smsSender;

        // private readonly Serilog.ILogger _logger;

        public UsersService(
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UsersService> logger,
            ISMSSender smsSender
        )
        {
            this.unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            this.config = config;
            this.mapper = mapper;
            _logger = logger;
            _smsSender = smsSender;
            // _logger = Log.ForContext<IUsersService>();
        }

        public async Task<ApiResponse<JwtAccessToken>> Login(LoginModel model)
        {
            ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();
            try
            {
                // model.Email = model.Email;
                var user = await unitOfWork.UserManager.GetUserByEmail(model.Email);

                if (user != null)
                {
                    bool res = await unitOfWork.UserManager.CheckPasswordAsync(
                        user,
                        model.Password
                    );

                    if (res)
                    {
                        if (user.Status == (int)UserStatus.Deactivated)
                        {
                            _logger.LogInformation("Deactivated {user} tried to log in", user.Id);
                            result.Succeeded = false;
                            result.Errors.Add(
                                "This account is deactivated. Please reach out for the support."
                            );
                            result.Errors.Add("هذا الحساب معطل. يرجى الاتصال بالدعم");
                            return result;
                        }
                        else
                        {
                            //Update last login status
                            user.LastLogin = DateTime.UtcNow;

                            var updateUserRes = await unitOfWork.UserManager.UpdateAsync(user);
                            await unitOfWork.SaveChangesAsync();
                            _logger.LogInformation("{user} logged in", user.Id);

                            var tokenResult = await GenerateUserToken(user);

                            return tokenResult;
                        }
                    }
                    else
                    {
                        _logger.LogInformation("{user} invalid password", user.Id);
                        result.Succeeded = false;
                        result.Errors.Add("Invalid Password");
                        result.Errors.Add("كلمة مرور غير صالحة.");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Login attempt - unregistered account");
                    result.Succeeded = false;
                    result.Errors.Add("This account is not registered");
                    result.Errors.Add("هذا الحساب غير مسجل");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while logging user in");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        // public async Task<ApiResponse<bool>> CreateRegistrationRequest(VerifyEmailModel model)
        // {
        //     ApiResponse<bool> result = new ApiResponse<bool>();
        //     try
        //     {
        //         //Verify existing registration request
        //         var existingRegistrationRequestQ = await unitOfWork.RegistrationRequestManager.GetAsync(t => t.Email == model.Email && t.Email == model.Email);
        //         var existingRegistrationRequest = existingRegistrationRequestQ.FirstOrDefault();

        //         if (existingRegistrationRequest is not null)
        //         {
        //             //Delete existing registration request
        //             var deletionRes = await unitOfWork.RegistrationRequestManager.RemoveAsync(existingRegistrationRequest);

        //             await unitOfWork.SaveChangesAsync();
        //         }

        //         //Generate OTP
        //         var otp = await HelperFunctions.GenerateOTP();

        //         //Create registration request
        //         RegistrationRequestDTO request = new RegistrationRequestDTO();
        //         request.CreateRequest(model.Email);

        //         var creationRes = await unitOfWork.RegistrationRequestManager.CreateAsync(
        //             mapper.Map<RegistrationRequest>(request));

        //         if (creationRes != null)
        //         {
        //             await unitOfWork.SaveChangesAsync();
        //             var otpSendResult = await _smsSender.SendSMS("Welcome to Holo street, your OTP is: otp", model.Email);

        //             _logger.LogInformation("OTP generated and sent successfully");

        //             result.Succeeded = true;
        //             result.ErrorType = ErrorType.NotRegistered;
        //             return result;
        //         }
        //         else
        //         {
        //             // Logging: Log the registration request creation failure
        //             _logger.LogError("Failed to create registration request");

        //             result.Succeeded = false;
        //             result.Errors.Add("An exception occured, please try again");
        //             result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
        //             return result;
        //         }

        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "An exception occurred while creating registration request");
        //         result.Succeeded = false;
        //         result.Errors.Add(ex.Message);
        //         result.ErrorType = ErrorType.SystemError;
        //         return result;
        //     }
        // }

        public async Task<ApiResponse<JwtAccessToken>> GenerateUserToken(ApplicationUser user)
        {
            ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();
            try
            {
                // Creating JWT Bearer Token .
                ClaimsIdentity claims = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim("given_name", user.FullName)
                    }
                );

                IList<string> userRoles = await unitOfWork.UserManager.GetRolesAsync(user);

                if (userRoles != null && userRoles.Count() > 0)
                {
                    foreach (string role in userRoles)
                    {
                        claims.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config.GetSection("Token:Key").Value)
                );
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var serverDateResult = DateTime.UtcNow;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    NotBefore = serverDateResult,
                    Expires = serverDateResult.AddDays(7), // Set Token Validity Period
                    SigningCredentials = creds,
                    IssuedAt = serverDateResult
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                result.Data = new JwtAccessToken
                {
                    Token = tokenHandler.WriteToken(token),
                    Expiration = token.ValidTo,
                    userStatus = user.Status
                };

                result.Data.RefreshToken = GenerateRefreshToken(result.Data.Token);

                //Update user refresh token
                user.RefreshToken = result.Data.RefreshToken;
                var updateUserTokenRes = await unitOfWork.UserManager.UpdateAsync(user);

                await unitOfWork.SaveChangesAsync();
                result.Succeeded = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        private string GenerateRefreshToken(string accessToken)
        {
            var accessTokenHandler = new JwtSecurityTokenHandler();
            var accessTokenJwt = accessTokenHandler.ReadJwtToken(accessToken);

            var claims = new List<Claim>();

            // Copy the necessary claims from the access token
            foreach (var claim in accessTokenJwt.Claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value));
            }

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Create a new ClaimsIdentity with the copied claims
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7), // Set Token Validity Period
                SigningCredentials = accessTokenJwt.SigningCredentials,
                IssuedAt = DateTime.UtcNow,
                Audience = accessTokenJwt.Audiences.FirstOrDefault(),
                Issuer = accessTokenJwt.Issuer
            };

            var refreshToken = accessTokenHandler.CreateToken(refreshTokenDescriptor);

            return accessTokenHandler.WriteToken(refreshToken);
        }

        public async Task<ApiResponse<JwtAccessToken>> RefreshAccessToken(RefreshTokenModel model)
        {
            ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);
                // Retrieve the current token used in the request
                var currentToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                    .ToString()
                    .Replace("Bearer ", "");

                var user = await unitOfWork.UserManager.GetUserById(userID);

                // Verify the refresh token and check if it matches the current token
                if (user.RefreshToken == model.RefreshToken)
                {
                    // Refresh the token and generate a new access token
                    var newToken = await GenerateUserToken(user);
                    return newToken;
                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Add("Invalid refresh token");
                    result.Errors.Add("رمز التحديث (Refresh Token) غير صالح.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        // private bool VerifyRefreshToken(string accessToken, string refreshToken)
        // {
        //     var generatedRefreshToken = GenerateRefreshToken(accessToken);

        //     if (generatedRefreshToken is not null && generatedRefreshToken == refreshToken)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }

        public async Task<ApiResponse<JwtAccessToken>> GoogleLogin(GoogleLogin model)
        {
            ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        model.AccessToken
                    );
                    var response = await client.GetAsync(
                        "https://www.googleapis.com/oauth2/v2/userinfo"
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var retreivedUserInfo =
                            JsonConvert.DeserializeObject<GoogleUserResponseModel>(content);

                        //Verify user's existence with such email
                        var user = await unitOfWork.UserManager.GetUserByEmail(
                            retreivedUserInfo.Email
                        );

                        //User exists, login
                        if (user is not null)
                        {
                            if (user.Status == (int)UserStatus.Deactivated)
                            {
                                _logger.LogInformation(
                                    "Deactivated {user} tried to log in",
                                    user.Id
                                );
                                result.Succeeded = false;
                                result.Errors.Add(
                                    "This account is deactivated. Please reach out for the support."
                                );
                                result.Errors.Add("هذا الحساب معطل. يرجى الاتصال بالدعم");
                                return result;
                            }
                            else
                            {
                                var tokenResult = await GenerateUserToken(user);
                                return tokenResult;
                                //Generate token for user
                            }
                        }
                        //Registration required
                        else
                        {
                            result.Succeeded = false;
                            result.ErrorType = ErrorType.NotRegistered;
                            return result;
                        }
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.ErrorType = ErrorType.SystemError;
                        result.Errors.Add(
                            $"Failed to retrieve user info from Google API. Status code: {response.StatusCode}"
                        );
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<JwtAccessToken>> AppleLogin(AppleLogin model)
        {
            ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();

            try
            {
                // Parse the received identity token
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(model.IdentityToken);

                // Get the Apple public keys
                var appleKeys = await GetApplePublicKeys();

                // Prepare the parameters
                var tokenParams = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = "https://appleid.apple.com",
                    IssuerSigningKeys = appleKeys,
                    ValidateAudience = true,
                    ValidAudience = "com.lconnect.app", // replace with your Apple client id
                };

                // Validate the token
                var claimsPrincipal = handler.ValidateToken(
                    model.IdentityToken,
                    tokenParams,
                    out var validatedToken
                );

                // Extract the email and sub claims from the token
                // string email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                // string appleId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                string emailClaimType =
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                string email = claimsPrincipal.Claims
                    .FirstOrDefault(c => c.Type == emailClaimType)
                    ?.Value;

                ApplicationUser? user = null;

                // If the email claim is present, use it to find or create the user's account
                if (!string.IsNullOrEmpty(email))
                {
                    user = await unitOfWork.UserManager.GetUserByEmail(email);
                }
                // // If the email claim is not present, use the sub claim (Apple ID) to identify the user
                // else if (!string.IsNullOrEmpty(appleId))
                // {
                //     user = await unitOfWork.UserManager.GetUserByAppleId(appleId);
                // }


                if (user is not null)
                {
                    if (user.Status == (int)UserStatus.Deactivated)
                    {
                        _logger.LogInformation("Deactivated {user} tried to log in", user.Id);
                        result.Succeeded = false;
                        result.Errors.Add(
                            "This account is deactivated. Please reach out for the support."
                        );
                        result.Errors.Add("هذا الحساب معطل. يرجى الاتصال بالدعم");
                        return result;
                    }
                    else
                    {
                        //Generate token for user
                        var tokenResult = await GenerateUserToken(user);
                        return tokenResult;
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.ErrorType = ErrorType.NotRegistered;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        private async Task<IEnumerable<SecurityKey>> GetApplePublicKeys()
        {
            var keys = new List<SecurityKey>();
            using var client = new HttpClient();

            var response = await client.GetAsync("https://appleid.apple.com/auth/keys");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var applePublicKeys = JsonConvert.DeserializeObject<ApplePublicKeysResponse>(
                jsonResponse
            );

            foreach (var key in applePublicKeys.keys)
            {
                var e = Base64UrlEncoder.DecodeBytes(key.e);
                var n = Base64UrlEncoder.DecodeBytes(key.n);

                var rsaParameters = new RSAParameters { Exponent = e, Modulus = n };

                var rsa = RSA.Create();
                rsa.ImportParameters(rsaParameters);

                var securityKey = new RsaSecurityKey(rsa) { KeyId = key.kid };

                keys.Add(securityKey);
            }

            return keys;
        }

        // public async Task<ApiResponse<string>> RequestEmailPasswordReset(
        //     RequestEmailPasswordResetModel model
        // )
        // {
        //     ApiResponse<string> result = new ApiResponse<string>();
        //     try
        //     {
        //         //Verify this phone number is not taken
        //         var Email = Regex.Replace(model.Email, @"\s", "");
        //         var EmailExists = await unitOfWork.UserManager.GetUserByEmail(Email);

        //         if (EmailExists is not null)
        //         {
        //             //Verify Email match

        //             if (EmailExists.Email == model.Email)
        //             {
        //                 //Create otp request
        //                 var otp = await HelperFunctions.GenerateOTP();

        //                 //TODO: UTC
        //                 DateTimeOffset localTime = DateTime.UtcNow;

        //                 OTPRequest request = new OTPRequest
        //                 {
        //                     UserID = EmailExists.Id,
        //                     OTP = otp,
        //                     CreationDate = DateTime.UtcNow,
        //                     ExpiryDate = DateTime.UtcNow.AddMinutes(2),
        //                     RequestType = (int)OTPRequestTypes.ForgotPassword,
        //                     IsUsed = false
        //                 };

        //                 var creationRes = await unitOfWork.OTPRequestManager.CreateAsync(request);

        //                 if (creationRes is not null)
        //                 {
        //                     await unitOfWork.SaveChangesAsync();
        //                     result.Data = otp;
        //                     //TODO: Send Email

        //                     _logger.LogInformation(
        //                         "{user} request password reset via email",
        //                         EmailExists.Id
        //                     );
        //                     result.Succeeded = true;
        //                     return result;
        //                 }
        //                 else
        //                 {
        //                     result.Succeeded = false;
        //                     result.Errors.Add("Exception occured");
        //                     result.Errors.Add("Exception occured");
        //                     _logger.LogError(
        //                         "Error creating password reset otp for user {user}",
        //                         EmailExists.Id
        //                     );
        //                     return result;
        //                 }
        //             }
        //             else
        //             {
        //                 result.Succeeded = false;
        //                 result.Errors.Add("The Email provided does not match this account");
        //                 result.Errors.Add("The Email provided does not match this account");
        //                 return result;
        //             }
        //         }
        //         else
        //         {
        //             result.Succeeded = false;
        //             result.Errors.Add("This phone number is not registered");
        //             result.Errors.Add("رقم الهاتف هذا غير مسجل");
        //             _logger.LogWarning("Password reset for inexistent phone number {Email}", Email);
        //             return result;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Exception resetting user password");
        //         result.Succeeded = false;
        //         result.Errors.Add(ex.Message);
        //         result.ErrorType = ErrorType.SystemError;
        //         return result;
        //     }
        // }

        //     public async Task<ApiResponse<JwtAccessToken>> UpdateUserPassword(
        //         UpdateUserPasswordModel model
        //     )
        //     {
        //         ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();
        //         try
        //         {
        //             var otpRequestQ = await unitOfWork.OTPRequestManager.GetAsync(
        //                 t =>
        //                     t.PasswordResetToken == model.Token
        //                     && t.RequestType == (int)OTPRequestTypes.ForgotPassword
        //                     && t.IsUsed
        //                     && (t.PasswordTokenIsUsed.HasValue && t.PasswordTokenIsUsed.Value == false)
        //                     && DateTime.UtcNow <= t.PasswordResetExpiryDate,
        //                 orderBy: q => q.OrderByDescending(x => x.CreationDate)
        //             );
        //             var otpRequest = otpRequestQ.FirstOrDefault();

        //             if (otpRequest is not null)
        //             {
        //                 var user = await unitOfWork.UserManager.GetUserById(otpRequest.UserID);
        //                 if (user is not null)
        //                 {
        //                     var hashedPassword = unitOfWork.UserManager.PasswordHasher.HashPassword(
        //                         user,
        //                         model.NewPassword
        //                     );

        //                     user.PasswordHash = hashedPassword;

        //                     await unitOfWork.UserManager.UpdateAsync(user);

        //                     otpRequest.PasswordTokenIsUsed = true;

        //                     await unitOfWork.OTPRequestManager.UpdateAsync(otpRequest);

        //                     await unitOfWork.SaveChangesAsync();

        //                     _logger.LogInformation("{user} - user password updated", otpRequest.UserID);

        //                     var tokenResult = await GenerateUserToken(user);

        //                     return tokenResult;
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError(
        //                         "{user} - error updating user password",
        //                         otpRequest.UserID
        //                     );
        //                     result.Succeeded = false;
        //                     result.Errors.Add("Exception occured");
        //                     result.Errors.Add("حدث استثناء");
        //                     return result;
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError("{user} - invalid or expired OTP");
        //                 result.Succeeded = false;
        //                 result.Errors.Add("Invalid or expired code");
        //                 result.Errors.Add("الرمز غير صالح أو انتهت صلاحيته");
        //                 return result;
        //             }
        //         }
        //         catch (Exception ex)
        //         {
        //             _logger.LogError(ex, "Exception updating user password");
        //             result.Succeeded = false;
        //             result.Errors.Add(ex.Message);
        //             result.ErrorType = ErrorType.SystemError;
        //             return result;
        //         }
        //     }
        // }
    }
}
