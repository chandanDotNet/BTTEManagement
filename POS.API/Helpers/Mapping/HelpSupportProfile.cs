using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportProfile'
    public class HelpSupportProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportProfile.HelpSupportProfile()'
        public HelpSupportProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportProfile.HelpSupportProfile()'
        {
            CreateMap<HelpSupport, HelpSupportDto>().ReverseMap();
        }
    }
}
