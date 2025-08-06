using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UserProfile'
    public class UserProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UserProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UserProfile.UserProfile()'
        public UserProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UserProfile.UserProfile()'
        {
            CreateMap<UserClaimDto, UserClaim>().ReverseMap();
            CreateMap<UserRoleDto, UserRole>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AddUserCommand, User>();
            CreateMap<ResetPasswordCommand, UserDto>();
            CreateMap<UpdateUserCommand, User>().ReverseMap();
        }
    }
}
