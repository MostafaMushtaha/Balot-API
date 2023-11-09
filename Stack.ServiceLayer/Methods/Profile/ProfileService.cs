using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.Entities.DomainEntities.Modules.Profile;
using Stack.Entities.Enums.Modules.User;
using Stack.Entities.DatabaseEntities.UserProfile;
using Stack.DTOs.Requests.UserProfile;
using Stack.Repository.Common;
using Stack.Entities.DatabaseEntities.User;
using Stack.ServiceLayer.Primitives;
using MediatR;
using Stack.ServiceLayer.DomainEvents;
using Stack.Entities.Enums.Modules.Notifications;
using Stack.Entities.DatabaseEntities.Modules.User;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Stack.ServiceLayer.Methods.UserProfiles
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly IMediator _mediator;
        private readonly IMediaUploader _mediaUploader;
        private readonly ILogger<IProfileService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileService(
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            IMediaUploader mediaUploader,
            ILogger<IProfileService> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            this.unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            this.config = config;
            this.mapper = mapper;
            _mediaUploader = mediaUploader;
            _mediator = mediator;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        #region Views

        public async Task<ApiResponse<ProfileViewModel>> ViewPersonalProfile()
        {
            ApiResponse<ProfileViewModel> result = new ApiResponse<ProfileViewModel>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    {
                        var profile = await unitOfWork.ProfileManager.ViewPersonalProfile(
                            userID
                        );

                        if (profile != null)
                        {
                            _logger.LogInformation("View personal profile {user} - {profile}", userID);
                            result.Succeeded = true;
                            result.Data = profile;
                            return result;
                        }
                        else
                        {
                            _logger.LogWarning("Profile not found {user} - {profile}", userID);
                            result.Succeeded = false;
                            // result.Errors.Add("Profile not found or an Exception occured");
                            result.Errors.Add("الملف الشخصي غير موجود أو حدث استثناء");
                            return result;
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: user ID not found");
                    result.Succeeded = false;
                    // result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception fetching profile");
                result.Succeeded = false;
                result.Errors.Add("استثناء أثناء جلب الملف الشخصي");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        #endregion

        #region Profile Updates

        public async Task<ApiResponse<string>> UpdateProfileImage(UpdateProfileImageModel model)
        {
            ApiResponse<string> result = new ApiResponse<string>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var profileQ = await unitOfWork.ProfileManager.GetAsync(
                        t => t.UserID == userID,
                        includeProperties: "User"
                    );
                    var profile = profileQ.FirstOrDefault();

                    if (profile != null)
                    {
                        string mediapath = "profile/profile-images";
                        var res = await _mediaUploader.UploadMedia(mediapath, model.Image);
                        if (res != null)
                        {
                            profile.Thumbnail = res;
                        }

                        var updateRes = await unitOfWork.ProfileManager.UpdateAsync(profile);
                        if (updateRes)
                        {
                            await unitOfWork.SaveChangesAsync();
                            result.Succeeded = true;
                            result.Data = profile.Thumbnail;
                            return result;
                        }
                        else
                        {
                            result.Succeeded = false;
                            result.Errors.Add("Exception occured");
                            result.Errors.Add("حدث استثناء");
                            return result;
                        }
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Errors.Add("Exception occured");
                        result.Errors.Add("حدث استثناءd");
                        return result;
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception updating profile image");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        #endregion
    }
}
