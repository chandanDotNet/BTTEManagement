using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class AppVersionUpdateProfile : Profile
    {
        public AppVersionUpdateProfile()
        {
            CreateMap<AppVersionUpdate, AppVersionUpdateDto>().ReverseMap();
        }
    }
}
