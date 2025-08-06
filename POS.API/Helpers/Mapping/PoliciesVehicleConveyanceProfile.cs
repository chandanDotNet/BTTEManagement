using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesVehicleConveyanceProfile'
    public class PoliciesVehicleConveyanceProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesVehicleConveyanceProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesVehicleConveyanceProfile.PoliciesVehicleConveyanceProfile()'
        public PoliciesVehicleConveyanceProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesVehicleConveyanceProfile.PoliciesVehicleConveyanceProfile()'
        {
            CreateMap<PoliciesVehicleConveyance, PoliciesVehicleConveyanceDto>().ReverseMap();          
            CreateMap<AddPoliciesVehicleConveyanceCommand, PoliciesVehicleConveyance>();
            CreateMap<UpdatePoliciesVehicleConveyanceCommand, PoliciesVehicleConveyance>();

        }
    }
}
