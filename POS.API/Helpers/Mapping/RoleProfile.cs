using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RoleProfile'
    public class RoleProfile: Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RoleProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RoleProfile.RoleProfile()'
        public RoleProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RoleProfile.RoleProfile()'
        {
            CreateMap<RoleClaim, RoleClaimDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<AddRoleCommand, Role>();
            CreateMap<UpdateRoleCommand, Role>();
        }
    }
}
