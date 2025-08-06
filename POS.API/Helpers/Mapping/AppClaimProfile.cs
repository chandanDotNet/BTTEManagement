using AutoMapper;
using POS.Data.Dto;
using System.Security.Claims;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'AppClaimProfile'
    public class AppClaimProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'AppClaimProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'AppClaimProfile.AppClaimProfile()'
        public AppClaimProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'AppClaimProfile.AppClaimProfile()'
        {
            CreateMap<AppClaimDto, Claim>()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ClaimType))
               .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ClaimValue))
               .ReverseMap();

          
        }
    }
}