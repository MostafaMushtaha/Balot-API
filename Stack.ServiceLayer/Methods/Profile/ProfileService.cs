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

        public ProfileService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor,
         IMediator mediator, IMediaUploader mediaUploader, ILogger<IProfileService> logger, IHttpClientFactory httpClientFactory)
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

        #region  Initialization

        //Register user account and respond with user ID
        //UserDetailsRegistrationModel model - Disabled
        public async Task<ApiResponse<bool>> InitializeUserProfile(ApplicationUser user)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                throw new NotImplementedException();
                //Create Social Profile

                // Entities.DatabaseEntities.UserProfile.Profile socialProfile =
                //     new Entities.DatabaseEntities.UserProfile.Profile
                //     {
                //         UserID = user.Id,
                //         PostsCount = 0,
                //         FollowersCount = 0,
                //         FollowingCount = 0,
                //         LikesCount = 0,
                //         CommentsCount = 0,
                //         Rating = 0,
                //         CreationDate = DateTime.UtcNow,
                //         Status = (int)ProfileStatus.Incomplete,
                //         CompletionPercentage = 15, //15% Completed
                //     };

                // var socialProfileCreationRes = await unitOfWork.ProfileManager.CreateAsync(
                //     socialProfile
                // );

                // if (socialProfileCreationRes != null)
                // {
                //     await unitOfWork.SaveChangesAsync();

                //     //Create profile settings
                //     ProfileSettings settings = new ProfileSettings
                //     {
                //         ProfileID = socialProfileCreationRes.ID,
                //         InvitationPrivacyMode = (int)ProfilePrivacyModes.Public,
                //         CreationDate = DateTime.UtcNow,
                //         CreatedBy = "System",
                //         PrivacyMode = (int)ProfilePrivacyModes.Public,
                //     };

                //     var settingsCreationRes = await unitOfWork.ProfileSettingsManager.CreateAsync(
                //         settings
                //     );
                //     if (settingsCreationRes != null)
                //     {
                //         //Create profile tutorial
                //         UserTutorials userTutorial = new UserTutorials
                //         {
                //             UserID = user.Id,
                //             Initial = false,
                //             CircleManagement = false,
                //             ApplicantFilteration = false,
                //             Spotlight = false
                //         };

                //         var creationRes = await unitOfWork.UserTutorialManager.CreateAsync(
                //             userTutorial
                //         );

                //         await unitOfWork.SaveChangesAsync();

                //         _logger.LogInformation("Profile initialized {user} - {profile}", user.Id, socialProfileCreationRes.ID);

                //         await SendToDiscord("User " + user.FullName + " has joined !");
                //         result.Succeeded = true;
                //         result.Data = true;
                //         return result;
                //     }
                //     else
                //     {
                //         _logger.LogError("Error creating profile settings {user} - {profile}", user.Id, socialProfileCreationRes.ID);
                //         result.Succeeded = false;
                //         result.Errors.Add("Error creating profile settings");
                //         result.Errors.Add("خطأ أثناء إنشاء إعدادات الملف الشخصي");
                //         return result;
                //     }
                // }
                // else
                // {
                //     _logger.LogError("Error creating user profile {user}", user.Id);
                //     result.Succeeded = false;
                //     result.Errors.Add("Failed to update user details, Please try again !");
                //     result.Errors.Add("فشل تحديث بيانات المستخدم ، يرجى المحاولة مرة أخرى!");
                //     return result;
                // }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception initializing profile");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task SendToDiscord(string messageContent)
        {
            var webhookUrl = "";
            var payload = new
            {
                content = messageContent
            };
            var httpClient = _httpClientFactory.CreateClient();

            var serializedPayload = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(webhookUrl, serializedPayload);
        }

        #endregion


        #region Views

        // Views profile - renders view model if profile is public or viewer is a follower
        // Logs profile view
        // public async Task<ApiResponse<ProfileViewModel>> ViewProfile(long profileID)
        // {
        //     ApiResponse<ProfileViewModel> result = new ApiResponse<ProfileViewModel>();
        //     try
        //     {
        //         var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

        //         if (userID != null)
        //         {
        //             var viewerProfileID = await unitOfWork.ProfileManager.GetProfileID(userID);

        //             if (viewerProfileID != 0)
        //             {
        //                 ProfileViewModel? profile = new ProfileViewModel();
        //                 //View user permission after verifying relation
        //                 if (viewerProfileID != profileID)
        //                 {
        //                     profile = await unitOfWork.ProfileManager.ViewProfile(
        //                         profileID,
        //                         viewerProfileID
        //                     );

        //                     if (!profile.IsFollowed)
        //                     {
        //                         var profileVerificationQ = await unitOfWork.ProfileManager.GetAsync(
        //                             t =>
        //                                 t.ID == profileID
        //                                 && t.ProfileSettings.PrivacyMode
        //                                     == (int)ProfilePrivacyModes.Private
        //                         );
        //                         var profileVerification = profileVerificationQ.FirstOrDefault();

        //                         if (profileVerification != null && profile.IsFollowed == false)
        //                         {
        //                             //Verify follow request

        //                             // profile.FollowRequestSent
        //                             var followRequestExistsQ =
        //                                 await unitOfWork.FollowRequestManager.GetAsync(
        //                                     t =>
        //                                         t.ProfileID == profileID
        //                                         && t.RequestorID == viewerProfileID
        //                                 );
        //                             var followRequestExists = followRequestExistsQ.FirstOrDefault();

        //                             //Follow request exists
        //                             if (followRequestExists != null)
        //                             {
        //                                 profile.FollowRequestSent = true;
        //                             }
        //                         }
        //                     }
        //                     _logger.LogInformation("View user profile {user} > {viewerProfile}", profileID, viewerProfileID);
        //                 }
        //                 //View personal profile
        //                 else
        //                 {
        //                     profile = await unitOfWork.ProfileManager.ViewPersonalProfile(
        //                         profileID
        //                     );
        //                     _logger.LogInformation("View personal profile {user} - {profile}", profileID, userID);
        //                 }

        //                 if (profile != null)
        //                 {
        //                     result.Succeeded = true;
        //                     result.Data = profile;
        //                     return result;
        //                 }
        //                 else
        //                 {
        //                     _logger.LogWarning("Profile not found {user} - {profile}", userID, profileID);
        //                     result.Succeeded = false;
        //                     result.Errors.Add("Profile not found or an Exception occured");
        //                     result.Errors.Add("الملف الشخصي غير موجود أو حدث استثناء");
        //                     return result;
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogWarning("Unauthorized access: profile ID not found");
        //                 result.Succeeded = false;
        //                 result.Errors.Add("Unauthorized");
        //                 result.Errors.Add("غير مُصرَّح به");
        //                 return result;
        //             }
        //         }
        //         else
        //         {
        //             _logger.LogWarning("Unauthorized access: user ID not found");
        //             result.Succeeded = false;
        //             result.Errors.Add("Unauthorized");
        //             result.Errors.Add("غير مُصرَّح به");
        //             return result;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Exception viewing profile");
        //         result.Succeeded = false;
        //         result.Errors.Add(ex.Message);
        //         result.ErrorType = ErrorType.SystemError;
        //         return result;
        //     }
        // }


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
                        //Todo: Verify old image existence and delete (method deprecation upgrade: store to archive db)

                        //Upload and update thumbnail
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
