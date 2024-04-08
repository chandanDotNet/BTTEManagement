using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.Data;
using AutoMapper;

namespace BTTEM.API.Helpers.Mapping
{
    public class ConveyanceProfile:Profile
    {

        public ConveyanceProfile()
        {
            CreateMap<Conveyance, ConveyanceDto>().ReverseMap();
            CreateMap<ConveyancesItem, ConveyancesItemDto>();
            //CreateMap<AddDepartmentCommand, Department>();
            //CreateMap<UpdateDepartmentCommand, Department>();
        }
    }
}
