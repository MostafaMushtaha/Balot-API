using AutoMapper;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DatabaseEntities.Notifications;
using Stack.Entities.DomainEntities.Notifications;

namespace Stack.API.AutoMapperConfig
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {
            CreateMap<Notification, NotificationDTO>()
            .ReverseMap();

            CreateMap<NotificationDTO, Notification>()
            .ReverseMap();
        }

    }

}
