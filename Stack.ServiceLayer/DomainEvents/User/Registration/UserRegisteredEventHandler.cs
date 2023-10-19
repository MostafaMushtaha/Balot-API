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
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly ILogger<UserRegisteredEventHandler> _logger;
        private readonly INotificationsService _notificationService;
        public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationtoken)
        {
            //Notify users with the user's phone number stored on device.
            //Get related users
            // NotificationDTO notification = new NotificationDTO().CreateUserRegistered()
            //Notify synced users
            _logger.LogInformation("a new user: {fullName} registered", notification.fullName);
        }
    }
}