using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Department.Commands;
using BTTEM.Data.Dto.PoliciesTravel;
using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TravelModeProfile'
    public class TravelModeProfile:Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TravelModeProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TravelModeProfile.TravelModeProfile()'
        public TravelModeProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TravelModeProfile.TravelModeProfile()'
        {
            CreateMap<TravelMode, TravelModeDto>().ReverseMap();
            CreateMap<ClassOfTravel, ClassOfTravelDto>().ReverseMap();
            CreateMap<AddTravelModeCommand, TravelMode>();
            CreateMap<UpdateTravelModeCommand, TravelMode>();
        }
    }
}
