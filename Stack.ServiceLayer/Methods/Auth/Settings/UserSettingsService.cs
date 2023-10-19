using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.Repository.Common;
using System.Text.RegularExpressions;
using Stack.ServiceLayer.Methods.Auth.User;
using Stack.DTOs.Requests.Modules.Settings;
using Stack.DTOs.Responses.Auth;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.Enums.Modules.Auth;
using Microsoft.Extensions.Logging;

namespace Stack.ServiceLayer.Methods.Auth
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly ILogger<IUserSettingsService> _logger;

        public UserSettingsService(
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<IUserSettingsService> logger
        )
        {
            this.unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            this.config = config;
            this.mapper = mapper;
            _logger = logger;
        }

        // public async Task<ApiResponse<bool>> UpdatePersonalDetails(UpdatePersonalDetailsModel model)
        // {
        //     ApiResponse<bool> result = new ApiResponse<bool>();
        //     try
        //     {
        //         var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

        //         if (userID is not null)
        //         {
        //             var user = await unitOfWork.UserManager.GetUserById(userID);

        //             if (user is not null)
        //             {
        //                 user.FirstName = model.FirstName;
        //                 user.LastName = model.LastName;
        //                 user.Birthdate = model.Birthdate;
        //                 user.FullName = user.FirstName + " " + user.LastName;

        //                 var updateRes = await unitOfWork.UserManager.UpdateAsync(user);

        //                 if (updateRes is not null)
        //                 {
        //                     _logger.LogInformation("Personal details updated {user}", userID);
        //                     await unitOfWork.SaveChangesAsync();
        //                     result.Succeeded = true;
        //                     return result;
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError("Error updating Personal details {user}", userID);
        //                     result.Succeeded = false;
        //                     result.Errors.Add("Exception occured");
        //                     result.Errors.Add("حدث استثناء");
        //                     return result;
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError(
        //                     "User not found - {user} - Update personal details",
        //                     userID
        //                 );
        //                 result.Succeeded = false;
        //                 result.Errors.Add("Unauthorized");
        //                 result.Errors.Add("غير مُصرَّح به");
        //                 return result;
        //             }
        //         }
        //         else
        //         {
        //             _logger.LogWarning("Unauthorized access: User ID not found");
        //             result.Succeeded = false;
        //             result.Errors.Add("Unauthorized");
        //             result.Errors.Add("غير مُصرَّح به");
        //             return result;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "An error occurred while updating personal detais");
        //         result.Succeeded = false;
        //         result.Errors.Add(ex.Message);
        //         result.ErrorType = ErrorType.SystemError;
        //         return result;
        //     }
        // }

        public async Task<ApiResponse<bool>> UpdatePassword(UpdatePasswordModel model)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID is not null)
                {
                    var user = await unitOfWork.UserManager.GetUserById(userID);

                    if (user is not null)
                    {
                        var passwordUpdateConfirmed =
                            await unitOfWork.UserManager.ChangePasswordAsync(
                                user,
                                model.Password,
                                model.NewPassword
                            );

                        if (passwordUpdateConfirmed is not null)
                        {
                            result.Succeeded = true;
                            return result;
                        }
                        else
                        {
                            _logger.LogError("Error updating password {user}", userID);
                            result.Succeeded = false;
                            result.Errors.Add("Exception occured");
                            result.Errors.Add("حدث استثناء");
                            return result;
                        }
                    }
                    else
                    {
                        _logger.LogError("User not found - {user} - Update password", userID);
                        result.Succeeded = false;
                        result.Errors.Add("Unauthorized");
                        result.Errors.Add("غير مُصرَّح به");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error updating password");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        // public async Task<ApiResponse<UserProfessionalDetailsModel>> GetUserCharacteristics()
        // {
        //     ApiResponse<UserProfessionalDetailsModel> result = new ApiResponse<UserProfessionalDetailsModel>();
        //     try
        //     {
        //         var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

        //         if (userID is not null)
        //         {
        //             throw new NotImplementedException();
        //             // var characteristics = await unitOfWork.HumanAttributesManager.GetUserCharacteristics(userID);
        //             // if (characteristics is not null)
        //             // {
        //             //     _logger.LogInformation("Fetched user characteristics -  {user}", userID);
        //             //     result.Succeeded = true;
        //             //     result.Data = characteristics;
        //             //     return result;
        //             //     // humanAttributes.Gender = model.Gender;
        //             //     // humanAttributes.Age = model.Age;
        //             //     // humanAttributes.EyeColor = model.EyeColor;
        //             //     // humanAttributes.HairColor = model.HairColor;
        //             //     // humanAttributes.Weight = model.Weight;
        //             //     // humanAttributes.Height = model.Height;
        //             //     // humanAttributes.Physique = model.Physique;
        //             //     // humanAttributes.SkinColor = model.SkinColor;

        //             // }
        //             // else
        //             // {
        //             //     _logger.LogError("Characteristics not found - {user}", userID);
        //             //     result.Succeeded = false;
        //             //     result.Errors.Add("Exception occured");
        //             //     result.Errors.Add("حدث استثناء");
        //             //     return result;
        //             // }
        //         }
        //         else
        //         {
        //             _logger.LogWarning("Unauthorized access: User ID not found");
        //             result.Succeeded = false;
        //             result.Errors.Add("Unauthorized");
        //             result.Errors.Add("غير مُصرَّح به");
        //             return result;
        //         }

        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "error fetching user characteristics");
        //         result.Succeeded = false;
        //         result.Errors.Add(ex.Message);
        //         result.ErrorType = ErrorType.SystemError;
        //         return result;
        //     }

        // }

        // public async Task<ApiResponse<bool>> UpdateUserCharacteristics(UserCharacteristicsModel model)
        // {
        //     ApiResponse<bool> result = new ApiResponse<bool>();
        //     try
        //     {
        //         var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

        //         if (userID is not null)
        //         {
        //             var userCharacteristicsQ = await unitOfWork.HumanAttributesManager.GetAsync(t => t.UserID == userID, includeProperties: "User");
        //             var userCharacteristics = userCharacteristicsQ.FirstOrDefault();

        //             if (userCharacteristics is not null)
        //             {
        //                 userCharacteristics.Age = model.Age;
        //                 userCharacteristics.EyeColor = model.EyeColor;
        //                 userCharacteristics.SkinColor = model.SkinColor;
        //                 userCharacteristics.Weight = model.Weight;
        //                 userCharacteristics.Height = model.Height;
        //                 userCharacteristics.Age = model.Age;
        //                 userCharacteristics.Physique = model.Physique;
        //                 userCharacteristics.Gender = model.Gender;

        //                 userCharacteristics.User.Country = model.Country;
        //                 userCharacteristics.User.City = model.City;
        //                 userCharacteristics.User.Address = model.Street;

        //                 var updateRes = await unitOfWork.UserManager.UpdateAsync(userCharacteristics.User);

        //                 if (updateRes is not null)
        //                 {
        //                     _logger.LogInformation("updated user characteristics -  {user}", userID);
        //                     await unitOfWork.SaveChangesAsync();
        //                     result.Succeeded = true;
        //                     return result;
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError("Error updating characteristics - {user}", userID);
        //                     result.Succeeded = false;
        //                     result.Errors.Add("Exception occured");
        //                     result.Errors.Add("حدث استثناء");
        //                     return result;

        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError("Characteristics not found - {user}", userID);
        //                 result.Succeeded = false;
        //                 result.Errors.Add("Exception occured");
        //                 result.Errors.Add("حدث استثناء");
        //                 return result;
        //             }
        //         }
        //         else
        //         {
        //             _logger.LogWarning("Unauthorized access: User ID not found");
        //             result.Succeeded = false;
        //             result.Errors.Add("Unauthorized");
        //             result.Errors.Add("غير مُصرَّح به");
        //             return result;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "error updating user characteristics");
        //         result.Succeeded = false;
        //         result.Errors.Add(ex.Message);
        //         result.ErrorType = ErrorType.SystemError;
        //         return result;
        //     }

        // }



        public async Task<ApiResponse<bool>> UpdateUserEmail(UpdateUserEmailModel model)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);
                if (userID is not null)
                {
                    var userCheck = await unitOfWork.UserManager.GetUserByEmail(model.Email);
                    if (userCheck is not null)
                    {
                        if (userCheck.Id == userID)
                        {
                            _logger.LogWarning(
                                "Email already registered to same account- {user}",
                                userID
                            );
                            result.Succeeded = false;
                            result.Data = false;
                            result.Errors.Add("Email is already registered to this account");
                            result.Errors.Add("البريد الإلكتروني مُسجل بالفعل لهذا الحساب.");
                            return result;
                        }
                        else
                        {
                            _logger.LogWarning(
                                "Email already registered to a different account- {user}",
                                userID
                            );
                            result.Succeeded = false;
                            result.Errors.Add(
                                "This Email is already registered to a different account"
                            );
                            result.Errors.Add("هذا البريد الإلكتروني مُسجل بالفعل على حساب مختلف.");
                            return result;
                        }
                    }
                    else
                    {
                        //Register account
                        //TODO: Send confirmation email, Currently automatically changes email

                        var user = await unitOfWork.UserManager.GetUserById(userID);

                        if (user is not null)
                        {
                            user.Email = model.Email;

                            var updateRes = await unitOfWork.UserManager.UpdateAsync(user);
                            if (updateRes is not null)
                            {
                                _logger.LogInformation(
                                    "Updated email- {user} {email}",
                                    userID,
                                    user.Email
                                );
                                await unitOfWork.SaveChangesAsync();
                                result.Succeeded = true;
                                return result;
                            }
                            else
                            {
                                _logger.LogError(
                                    "Error updating email- {user} {email}",
                                    userID,
                                    user.Email
                                );
                                result.Succeeded = false;
                                result.Errors.Add("An exception occured, please try again");
                                result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
                                return result;
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Unauthorized access: User not found");
                            result.Succeeded = false;
                            result.Errors.Add("Unauthorized");
                            result.Errors.Add("غير مُصرَّح به");
                            return result;
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error updating user email");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        //Deactivate user account ()
        public async Task<ApiResponse<bool>> DeactivateUserAccount()
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);
                if (userID is not null)
                {
                    var user = await unitOfWork.UserManager.GetUserById(userID);
                    if (user is not null)
                    {
                        user.Status = (int)UserStatus.Deactivated;

                        //Detach phone number
                        user.PhoneNumber = "";
                        // user.phoneNumberSynonym = "";

                        var updateRes = await unitOfWork.UserManager.UpdateAsync(user);

                        if (updateRes is not null)
                        {
                            _logger.LogInformation("Account deactivated - {user}", userID);
                            await unitOfWork.SaveChangesAsync();
                            result.Succeeded = true;
                            return result;
                        }
                        else
                        {
                            _logger.LogWarning("Error deactivating user account {user}", userID);
                            result.Succeeded = false;
                            result.Errors.Add("An exception occured, please try again");
                            result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
                            return result;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Unauthorized access: User not found");
                        result.Succeeded = false;
                        result.Errors.Add("An exception occured, please try again");
                        result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error deactivating user account");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
    }
}
