using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.Data;
using AutoMapper;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class ConveyanceProfile:Profile
    {

        public ConveyanceProfile()
        {
            CreateMap<Conveyance, ConveyanceDto>().ReverseMap();
            CreateMap<ConveyancesItem, ConveyancesItemDto>().ReverseMap();
            CreateMap<AddConveyanceCommand, Conveyance>();
            CreateMap<UpdateConveyanceCommand, Conveyance>();
        }
    }
}
