using AutoMapper;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DomainEntities.Auth.Registration;

namespace Stack.API.AutoMapperConfig
{
    public class RegistrationMapperProfile : Profile
    {
        public RegistrationMapperProfile()
        {
            CreateMap<RegistrationRequest, RegistrationRequestDTO>()
            .ReverseMap();

            CreateMap<RegistrationRequestDTO, RegistrationRequest>()
            .ReverseMap();
        }

    }

}
