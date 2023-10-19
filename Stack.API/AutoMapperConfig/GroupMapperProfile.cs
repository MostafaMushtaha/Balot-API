using AutoMapper;
using Stack.Entities.DomainEntities.Auth;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.User;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DomainEntities.Groups;

namespace Stack.API.AutoMapperConfig
{
    public class GroupMapperProfile : Profile
    {
        public GroupMapperProfile()
        {

            CreateMap<Group, GroupDTO>()
            .ReverseMap();

        }

    }

}
