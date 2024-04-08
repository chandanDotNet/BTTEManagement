using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Department.Commands;
using BTTEM.Data.Dto.PoliciesTravel;
using AutoMapper;

namespace BTTEM.API.Helpers.Mapping
{
    public class TravelModeProfile:Profile
    {

        public TravelModeProfile()
        {
            CreateMap<TravelMode, TravelModeDto>().ReverseMap();
            CreateMap<ClassOfTravel, ClassOfTravelDto>();
            //CreateMap<AddDepartmentCommand, Department>();
            //CreateMap<UpdateDepartmentCommand, Department>();
        }
    }
}
