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
    public class GroupMemberProfile : Profile
    {
        public GroupMemberProfile()
        {
            // Map between Group_Member and Group_MemberDTO.
            CreateMap<Group_Member, Group_MemberDTO>()
                .ReverseMap();

            // Map from string to Group_Member for the UserID property.
            CreateMap<string, Group_Member>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src));
        }
    }
}
