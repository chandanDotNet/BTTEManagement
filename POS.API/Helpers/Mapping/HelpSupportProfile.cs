using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class HelpSupportProfile : Profile
    {
        public HelpSupportProfile()
        {
            CreateMap<HelpSupport, HelpSupportDto>().ReverseMap();
        }
    }
}
