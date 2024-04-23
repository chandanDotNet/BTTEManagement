using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class PoliciesVehicleConveyanceProfile :Profile
    {

        public PoliciesVehicleConveyanceProfile()
        {
            CreateMap<PoliciesVehicleConveyance, PoliciesVehicleConveyanceDto>().ReverseMap();          
            CreateMap<AddPoliciesVehicleConveyanceCommand, PoliciesVehicleConveyance>();
            CreateMap<UpdatePoliciesVehicleConveyanceCommand, PoliciesVehicleConveyance>();

        }
    }
}
