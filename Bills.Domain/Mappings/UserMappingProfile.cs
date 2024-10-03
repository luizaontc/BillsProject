using AutoMapper;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;

namespace Bills.Domain.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

        }
    }
}
