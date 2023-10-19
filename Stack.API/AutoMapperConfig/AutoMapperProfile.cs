using AutoMapper;
using Stack.Entities.DomainEntities.Auth;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.User;
using Stack.Entities.DatabaseEntities.Modules.User;

namespace Stack.API.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<ApplicationUser, ApplicationUserDTO>()
            .ReverseMap();

            CreateMap<ApplicationRole, ApplicationRoleDTO>()
            .ReverseMap();

            CreateMap<UserDevicesDTO, UserDevice>()
            .ReverseMap();

            CreateMap<UserDevice, UserDevicesDTO>()
            .ReverseMap();

        }

    }

}
