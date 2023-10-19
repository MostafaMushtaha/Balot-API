using AutoMapper;
using Microsoft.Extensions.Configuration;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Stack.Repository.Common;
using System.IdentityModel.Tokens.Jwt;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.Entities.Enums.Modules.Auth;
using Stack.Entities.DatabaseEntities.User;
using System.Text.RegularExpressions;
using Stack.DTOs.Models;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.ServiceLayer.Methods.Auth.Registration;
using Stack.ServiceLayer.DomainEvents;
using MediatR;
using Stack.Entities.DomainEntities.Auth.Registration;
using Stack.ServiceLayer.Primitives;
using Stack.ServiceLayer.Methods.UserProfiles;
using Stack.ServiceLayer.Methods.Auth.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Stack.Entities.DomainEntities.Auth;

namespace Stack.ServiceLayer.Methods.Auth
{
    public class RegistrationService : IRegistrationservice
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly IMediator _mediator;
        private readonly ISMSSender _smsSender;
        private IProfileService _profileService;
        private IUsersService _usersService;
        private ILogger<IRegistrationservice> _logger;

        public RegistrationService(
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            ISMSSender smsSender,
            IProfileService profileService,
            IUsersService usersService,
            ILogger<IRegistrationservice> logger
        )
        {
            this.unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            this.config = config;
            this.mapper = mapper;
            _mediator = mediator;
            _smsSender = smsSender;
            _profileService = profileService;
            _usersService = usersService;
            _logger = logger;
        }

        #region Initial registration - Duplication check & OTP verification
        //Verify OTP input - Creates user account via phone number and deletes existing registration request


        //Finalizes registration (user password) and refreshes token
        public async Task<ApiResponse<JwtAccessToken>> FinalizeRegistration(RegistrationModel model)
        {
            ApiResponse<JwtAccessToken> result = new ApiResponse<JwtAccessToken>();

            try
            {
                var existingUser = await unitOfWork.UserManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Email already exists");
                    result.Succeeded = false;
                    result.Errors.Add("Email already exists");
                    result.Errors.Add("البريد الإلكتروني موجود بالفعل");
                    return result;
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.Fullname,
                    Gender = model.Gender,
                    ReferenceNumber = GenerateUniqueReference()
                };
                var createResult = await unitOfWork.UserManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    _logger.LogInformation("{user} - user created successfully", model.UserName);
                    var tokenResult = await _usersService.GenerateUserToken(user);
                    return tokenResult;
                }
                else
                {
                    _logger.LogError("{user} - error creating user", model.UserName);
                    result.Succeeded = false;
                    result.Errors.AddRange(createResult.Errors.Select(e => e.Description));
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Logging: Log the exception details
                _logger.LogError(ex, "An error occurred during registration");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        private string GenerateUniqueReference()
        {
            return DateTime.UtcNow.ToString("HHmmssff") + new Random().Next(10, 99).ToString();
        }

        #endregion


        #region Google social registration

        public async Task<ApiResponse<string>> Google_VerifyPhoneNumberRegistration(
            Google_VerifyPhoneNumberModel model
        )
        {
            ApiResponse<string> result = new ApiResponse<string>();
            try
            {
                //Reformat phone number
                model.PhoneNumber = Regex.Replace(model.PhoneNumber, @"\s", "");
                model.PhoneNumberSynonym = Regex.Replace(model.PhoneNumberSynonym, @"\s", "");

                //Verify Duplicate phone number
                var phoneNumberExists = await unitOfWork.UserManager.CheckPhoneNumbersExistence(
                    model.PhoneNumber
                );

                //Proceed with OTP generation if phone number does not exist
                if (!phoneNumberExists)
                {
                    //Generate OTP
                    var otp = await HelperFunctions.GenerateOTP();

                    //Verify existing registration request existence
                    var registrationRequest =
                        await unitOfWork.RegistrationRequestManager.GetRegistrationRequestByPhoneNumber(
                            model.PhoneNumber
                        );

                    //Re-request OTP
                    if (registrationRequest != null)
                    {
                        RegistrationRequestDTO registrationRequestDomain =
                            mapper.Map<RegistrationRequestDTO>(registrationRequest);
                        registrationRequestDomain.Google_UpdateRequest(model, otp);

                        var updateRes = await unitOfWork.RegistrationRequestManager.UpdateAsync(
                            mapper.Map<RegistrationRequest>(registrationRequest)
                        );
                        if (updateRes)
                        {
                            await unitOfWork.SaveChangesAsync();

                            //TODO: Send OTP -- Disabled until sms sender works
                            // var otpSendResult = await _smsSender.SendSMS("Hello, your OTP is: otp", registrationRequest.PhoneNumber);

                            result.Succeeded = true;
                            result.Data = otp;
                            return result;
                        }
                        else
                        {
                            result.Succeeded = false;
                            result.Errors.Add("An exception occured, please try again");
                            result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
                            return result;
                        }
                    }
                    //Create new registration request
                    else
                    {
                        //Create registration request
                        RegistrationRequestDTO request = new RegistrationRequestDTO();
                        request.Google_CreateRequest(model, otp);

                        var creationRes = await unitOfWork.RegistrationRequestManager.CreateAsync(
                            mapper.Map<RegistrationRequest>(request)
                        );
                        if (creationRes != null)
                        {
                            await unitOfWork.SaveChangesAsync();

                            //TODO: Send OTP -- Disabled until sms sender works
                            // var otpSendResult = await _smsSender.SendSMS("Hello, your OTP is: otp", registrationRequest.PhoneNumber);

                            result.Succeeded = true;
                            result.Data = otp;
                            return result;
                        }
                        else
                        {
                            result.Succeeded = false;
                            result.Errors.Add("An exception occured, please try again");
                            result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
                            return result;
                        }
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Add(
                        "This phone number is already linked to an account, would you like to login instead?"
                    );
                    result.Errors.Add(
                        "رقم الهاتف هذا مرتبط بالفعل بحساب ، هل ترغب في تسجيل الدخول بدلاً من ذلك؟"
                    );
                    result.ErrorType = ErrorType.AlreadyRegistered;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying googe otp registration");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        #endregion
    }
}
