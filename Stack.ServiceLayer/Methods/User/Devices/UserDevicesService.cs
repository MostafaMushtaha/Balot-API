
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.Repository.Common;
using Stack.Entities.DomainEntities.User;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.DTOs.Requests.Modules.Auth;
using Microsoft.Extensions.Logging;

namespace Stack.ServiceLayer.Methods.User
{
    public class UserDevicesService : IUserDevicesService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<IUserDevicesService> _logger;

        public UserDevicesService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<IUserDevicesService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public async Task<ApiResponse<bool>> RegisterDevice(UserDeviceModel model)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);
                if (userID != null)
                {
                    var userDeviceQ = await unitOfWork.UserDeviceManager.GetAsync(t => t.UserID == userID && t.Token == model.DeviceToken);
                    var userDevice = userDeviceQ.FirstOrDefault();
                    if (userDevice is not null)
                    {
                        //Activate device
                        if (userDevice.IsActive == false)
                        {
                            userDevice.IsActive = true;
                            //Update device activity status

                            var updateRes = await unitOfWork.UserDeviceManager.UpdateAsync(userDevice);

                            if (updateRes)
                            {
                                _logger.LogInformation("{user} Device toggled", userID);
                                await unitOfWork.SaveChangesAsync();
                                result.Succeeded = true;
                                return result;
                            }
                            else
                            {
                                _logger.LogError("Error registering user device", userID);
                                result.Succeeded = false;
                                result.Errors.Add("Exception occured whilst activating user device");
                                result.Errors.Add("حدث استثناء أثناء تنشيط جهاز المستخدم.");
                                return result;
                            }
                        }
                        else
                        {
                            _logger.LogInformation("{user} Device registered", userID);
                            result.Succeeded = true;
                            return result;
                        }
                    }
                    else
                    {
                        //Verify existing active devices
                        var existingActiveDevicesQ = await unitOfWork.UserDeviceManager.GetAsync(t => t.UserID == userID && t.IsActive == true);
                        var existingActiveDevices = existingActiveDevicesQ.ToList();

                        if (existingActiveDevices is not null && existingActiveDevices.Count > 0)
                        {
                            //Deactivate previously existing device
                            for (int i = 0; i < existingActiveDevices.Count; i++)
                            {
                                var existingDevice = existingActiveDevices[i];
                                existingDevice.IsActive = false;
                                var updateRes = await unitOfWork.UserDeviceManager.UpdateAsync(existingDevice);
                            }

                            //Create new device
                            UserDevicesDTO device = new UserDevicesDTO(model.DeviceToken, userID, model.IsAndroid);
                            var creationResult = await unitOfWork.UserDeviceManager.CreateAsync(mapper.Map<UserDevice>(device));

                            if (creationResult != null)
                            {
                                _logger.LogInformation("{user} Device registered and toggled", userID);
                                await unitOfWork.SaveChangesAsync();
                                result.Succeeded = true;
                                result.Data = true;
                                return result;
                            }
                            else
                            {
                                _logger.LogInformation("{user} Error registering device", userID);
                                result.Succeeded = false;
                                result.Errors.Add("Error registering device token");
                                result.Errors.Add("خطأ في تسجيل رمز الجهاز (Device Token).");
                                return result;
                            }
                        }
                        else
                        {
                            //Create new device
                            UserDevicesDTO device = new UserDevicesDTO(model.DeviceToken, userID, model.IsAndroid);
                            var creationResult = await unitOfWork.UserDeviceManager.CreateAsync(mapper.Map<UserDevice>(device));

                            if (creationResult != null)
                            {
                                await unitOfWork.SaveChangesAsync();
                                _logger.LogInformation("{user} Device registered and toggled", userID);
                                result.Succeeded = true;
                                result.Data = true;
                                return result;
                            }
                            else
                            {
                                _logger.LogInformation("{user} Error registering device", userID);
                                result.Succeeded = false;
                                result.Errors.Add("Error registering device token");
                                result.Errors.Add("خطأ في تسجيل رمز الجهاز (Device Token).");
                                return result;
                            }
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
                _logger.LogError(ex, "Exception registering user device");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> DeactivateDevice()
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);
                if (userID != null)
                {
                    var userDeviceQ = await unitOfWork.UserDeviceManager.GetAsync(t => t.UserID == userID && t.IsActive == true);
                    var userDevice = userDeviceQ.FirstOrDefault();
                    if (userDevice is not null)
                    {
                        //Deactivate device
                        userDevice.IsActive = false;


                        var updateRes = await unitOfWork.UserDeviceManager.UpdateAsync(userDevice);

                        if (updateRes)
                        {
                            var user = await unitOfWork.UserManager.GetUserById(userID);
                            user.LastLogin = DateTime.UtcNow;
                            var updateUserRes = await unitOfWork.UserManager.UpdateAsync(user);

                            await unitOfWork.SaveChangesAsync();
                            _logger.LogInformation("{user} Device deactivated", userID);
                            result.Succeeded = true;
                            result.Data = true;
                            return result;
                        }
                        else
                        {
                            _logger.LogError("{user} Error deactivating device", userID);
                            result.Succeeded = false;
                            result.Errors.Add("Exception occured, unable to disable user device");
                            result.Errors.Add("حدث استثناء، غير قادر على تعطيل جهاز المستخدم.");
                            return result;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("{user} Error deactivating device - device not found {device}");
                        result.Succeeded = false;
                        result.Errors.Add("Exception occured, device not found");
                        result.Errors.Add("حدث استثناء، لم يتم العثور على الجهاز.");
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
                _logger.LogError(ex, "Exception deactivating user device");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

    }

}
