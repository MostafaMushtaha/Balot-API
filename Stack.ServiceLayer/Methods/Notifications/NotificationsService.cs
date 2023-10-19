
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Stack.Core;
using Stack.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stack.ServiceLayer.Firebase;
using Stack.Entities.DomainEntities.Notifications;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Repository.Common;
using Stack.DTOs.Responses.Notifications;
using FirebaseAdmin.Messaging;
using Notification = Stack.Entities.DatabaseEntities.Notifications.Notification;
using NotificationFCM = FirebaseAdmin.Messaging.Notification;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Stack.Entities.Enums.Modules.Notifications;

namespace Stack.ServiceLayer.Methods.Notifications
{

    public class NotificationsService : INotificationsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<NotificationsService> _logger;

        public NotificationsService(IOptions<FcmNotificationSetting> settings, IUnitOfWork unitOfWork, IConfiguration config,
         IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, ILogger<NotificationsService> logger)
        {
            this.unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            this.config = config;
            this.mapper = mapper;
            _fcmNotificationSetting = settings.Value;
            _env = env;
            _logger = logger;
        }

        //Automatically sends push notifications
        public async Task<ApiResponse<bool>> CreateNotification(NotificationDTO notification)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();

            try
            {
                if (notification.IsPushOnly == null)
                {
                    var creationResult = await unitOfWork.NotificationsManager.CreateAsync(mapper.Map<Notification>(notification));
                    if (creationResult != null)
                    {
                        await unitOfWork.SaveChangesAsync();
                        //Push notification
                        await SendPushNotification(notification);

                        _logger.LogInformation("Notification created and pushed {notification} - {user} ", notification.ID, notification.UserID);
                        result.Succeeded = true;
                        return result;
                    }
                    else
                    {
                        _logger.LogError("Error creating notification - {user}", notification.UserID);
                        result.Succeeded = false;
                        result.Errors.Add("Exception occured whilst creating notification");
                        result.Errors.Add("حدث استثناء أثناء إنشاء الإشعار");
                        return result;
                    }
                }
                else
                {
                    await SendPushNotification(notification);
                    _logger.LogInformation("Push only Notification sent {notification} - {user} ", notification.ID, notification.UserID);
                    result.Succeeded = true;
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception creating notification");
                result.Succeeded = false;
                return result;
            }
        }


        public async Task<ApiResponse<bool>> ReadNotification(long ID)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var readResult = await unitOfWork.NotificationsManager.SetNotificationRead(ID, userID);
                    if (readResult)
                    {
                        _logger.LogInformation("Notification {notification} read -  {user}", userID, ID);
                        result.Succeeded = true;
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning("Error reading notification {user} - {notification}", userID, ID);
                        result.Succeeded = false;
                        result.Errors.Add("Exception occured whilst updating notification details");
                        result.Errors.Add("حدث استثناء أثناء تحديث تفاصيل الإشعار");
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
                _logger.LogError(ex, "Exception reading notification");
                result.Succeeded = false;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> ReadNotifications()
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var readResult = await unitOfWork.NotificationsManager.SetNotificationsRead(userID);
                    if (readResult)
                    {
                        _logger.LogInformation("All notifications read -  {user}", userID);
                        result.Succeeded = true;
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning("Error reading notifications {user}", userID);
                        result.Succeeded = false;
                        result.Errors.Add("Exception occured whilst updating notification details");
                        result.Errors.Add("حدث استثناء أثناء تحديث تفاصيل الإشعار");
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
                _logger.LogError(ex, "Exception reading notifications");
                result.Succeeded = false;
                return result;
            }
        }


        public async Task<ApiResponse<int>> GetUnreadNotificationsCount()
        {
            ApiResponse<int> result = new ApiResponse<int>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    int count = await unitOfWork.NotificationsManager.GetUnreadNotificationsCount(userID);
                    _logger.LogInformation("Unread notifications fetched -  {user}", userID);
                    result.Succeeded = true;
                    result.Data = count;
                    return result;
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
                _logger.LogError(ex, "Exception fetching unread notifications");
                result.Succeeded = false;
                return result;
            }
        }


        public async Task<ApiResponse<NotificationGroupViewModel>> GetNotifications()
        {
            ApiResponse<NotificationGroupViewModel> result = new ApiResponse<NotificationGroupViewModel>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    NotificationGroupViewModel notifications = await unitOfWork.NotificationsManager.GetNotifications(userID);
                    if (notifications is not null)
                    {
                        _logger.LogInformation("Notifications fetched -  {user}", userID);
                        result.Succeeded = true;
                        result.Data = notifications;
                        return result;
                    }
                    else
                    {
                        _logger.LogInformation("No notifications fetched -  {user}", userID);
                        result.Succeeded = false;
                        result.Errors.Add("No notifications");
                        result.Errors.Add("No notifications");
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
                _logger.LogError(ex, "Exception fetching notifications");
                result.Succeeded = false;
                return result;
            }
        }


        public async Task<ApiResponse<bool>> SendPushNotification(NotificationDTO notificationModel)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                // Initialize the Firebase Admin SDK if it's not already initialized
                if (FirebaseApp.DefaultInstance == null)
                {
                    string pathToServiceAccountKey = Path.Combine(_env.ContentRootPath, "Properties", "lconnect-e9088-firebase-adminsdk-sqtr7-77a6198eb3.json");
                    // string pathToServiceAccountKey = "../Stack.API/Properties/lconnect-e9088-firebase-adminsdk-sqtr7-77a6198eb3.json";
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromFile(pathToServiceAccountKey),
                    });
                }

                UserDevice userDevice = await unitOfWork.UserDeviceManager.GetUserDevices(notificationModel.UserID);

                if (userDevice != null)
                {
                    string sound = "sparkle_sound_effect";
                    if (notificationModel.Type == (int)NotificationTypes.ApplicantAccepted)
                    {
                        sound = "sparkle_sound_effect";
                    }

                    var message = new Message
                    {
                        Token = userDevice.Token,
                        Notification = new NotificationFCM
                        {
                            Title = notificationModel.Title,
                            Body = notificationModel.Message
                        },
                        Android = new AndroidConfig()
                        {
                            Notification = new AndroidNotification()
                            {
                                // DefaultSound = true,
                                Sound = sound,
                                ChannelId = "customChannel" // ID of the notification channel created in the Android app.

                            }
                        },
                        Apns = new ApnsConfig
                        {
                            Aps = new Aps
                            {
                                Sound = sound + ".wav",
                                Alert = new ApsAlert
                                {
                                    Title = notificationModel.Title,
                                    Body = notificationModel.Message,
                                }
                            }
                        },
                        Data = new Dictionary<string, string>
                            {
                                { "ReferenceID", notificationModel.ReferenceID },
                                { "Type", notificationModel.Type.ToString() }
                            }
                    };

                    try
                    {
                        // Send the message and get the response
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                        _logger.LogInformation("Firebase response sent " + response);
                        result.Succeeded = true;
                        return result;
                    }
                    catch (FirebaseException ex)
                    {
                        Console.WriteLine("Error sending message: " + ex.Message);
                        _logger.LogError(ex, "Error in SendPushNotification method.");
                        result.Succeeded = false;
                        return result;
                    }
                }
                else
                {
                    _logger.LogInformation("Push notification not sent - No devices active {user}", notificationModel.UserID);
                    result.Succeeded = false;
                    result.Errors.Add("No devices found");
                    result.Errors.Add("لم يتم العثور على أي أجهزة");
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception sending push notification");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                return result;
            }
        }


    }

}
