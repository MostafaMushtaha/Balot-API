using Stack.ServiceLayer.Methods.Auth;
using Stack.ServiceLayer.Methods.Auth.Registration;
using Stack.ServiceLayer.Methods.Auth.User;
using Stack.ServiceLayer.Methods.Groups;
using Stack.ServiceLayer.Methods.Notifications;
using Stack.ServiceLayer.Methods.System;
using Stack.ServiceLayer.Methods.User;
using Stack.ServiceLayer.Methods.UserProfiles;
using Stack.ServiceLayer.Methods.Users;

namespace Stack.API.Extensions
{
    public static class ServiceExtensions
    {

        public static void AddServiceMethods(this IServiceCollection caller)
        {
            caller.AddScoped<IUsersService, UsersService>();
            caller.AddScoped<IGroupsService, GroupsService>();
            caller.AddScoped<IGroupsManagemenetService, GroupsManagemenetService>();
            caller.AddScoped<IRegistrationservice, RegistrationService>();
            caller.AddScoped<IUserDevicesService, UserDevicesService>();
            caller.AddScoped<INotificationsService, NotificationsService>();
            caller.AddScoped<IProfileService, ProfileService>();
            caller.AddScoped<IAppVersioningService, AppVersioningService>();
            caller.AddScoped<IUserSettingsService, UserSettingsService>();
            caller.AddScoped<ISystemServicesService, SystemServicesService>();
            caller.AddScoped<IFriendsService, FriendsService>();


            caller.AddHttpClient();
        }

    }

}
