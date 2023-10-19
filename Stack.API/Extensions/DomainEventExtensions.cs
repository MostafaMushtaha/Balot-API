
using MediatR;
using Stack.ServiceLayer.DomainEvents;

namespace Stack.API.Extensions
{
    public static class DomainEventExtensions
    {

        public static void AddDomainEvents(this IServiceCollection caller)
        {
            caller.AddScoped<INotificationHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
            caller.AddScoped<INotificationHandler<InactivityNotifierEvent>, InactivityNotifierEventHandler>();
        }

    }

}
