using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'AppVersionUpdateProfile'
    public class AppVersionUpdateProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'AppVersionUpdateProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'AppVersionUpdateProfile.AppVersionUpdateProfile()'
        public AppVersionUpdateProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'AppVersionUpdateProfile.AppVersionUpdateProfile()'
        {
            CreateMap<AppVersionUpdate, AppVersionUpdateDto>().ReverseMap();
        }
    }
}
