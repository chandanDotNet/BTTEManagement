using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class BusinessAreaProfile : Profile
    {
        public BusinessAreaProfile()
        {
            CreateMap<BusinessArea, BusinessAreaDto>().ReverseMap();
        }
    }
}
