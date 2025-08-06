using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.Data;
using AutoMapper;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ConveyanceProfile'
    public class ConveyanceProfile:Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ConveyanceProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ConveyanceProfile.ConveyanceProfile()'
        public ConveyanceProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ConveyanceProfile.ConveyanceProfile()'
        {
            CreateMap<Conveyance, ConveyanceDto>().ReverseMap();
            CreateMap<ConveyancesItem, ConveyancesItemDto>().ReverseMap();
            CreateMap<AddConveyanceCommand, Conveyance>();
            CreateMap<UpdateConveyanceCommand, Conveyance>();
        }
    }
}
