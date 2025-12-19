using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class MMTCityProfile : Profile
    {
        public MMTCityProfile()
        {
            CreateMap<MMTCity, MMTCityDto>().ReverseMap();
        }
    }
}
