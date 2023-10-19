using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Stack.Entities.DomainEntities.Notifications;
using Stack.ServiceLayer.Methods.Notifications;

namespace Stack.ServiceLayer.DomainEvents
{
    public class InactivityNotifierEventHandler : INotificationHandler<InactivityNotifierEvent>
    {
        private readonly ILogger<InactivityNotifierEventHandler> _logger;
        private readonly INotificationsService _notificationService;
        public InactivityNotifierEventHandler(ILogger<InactivityNotifierEventHandler> logger, INotificationsService notificationsService)
        {
            _logger = logger;
            _notificationService = notificationsService;
        }

        public async Task Handle(InactivityNotifierEvent notificationEvent, CancellationToken cancellationtoken)
        {
            NotificationDTO notification = new NotificationDTO();

            notification.InactivityNotification(notificationEvent.UserID, notificationEvent.Message);

            await _notificationService.CreateNotification(notification);
            _logger.LogInformation("{message} to {user}", notificationEvent.Message, notificationEvent.UserID);
        }
    }
}