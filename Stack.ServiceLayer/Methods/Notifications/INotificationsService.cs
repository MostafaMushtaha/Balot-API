using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stack.DTOs;
using Stack.DTOs.Notifications;
using Stack.DTOs.Responses.Notifications;
using Stack.Entities.DomainEntities.Notifications;

namespace Stack.ServiceLayer.Methods.Notifications
{
    public interface INotificationsService
    {
        public Task<ApiResponse<bool>> CreateNotification(NotificationDTO notification);
        public Task<ApiResponse<bool>> ReadNotification(long ID);
        public Task<ApiResponse<bool>> ReadNotifications();
        public Task<ApiResponse<int>> GetUnreadNotificationsCount();
        public Task<ApiResponse<NotificationGroupViewModel>> GetNotifications();
    }
}