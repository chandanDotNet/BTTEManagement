using AutoMapper;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class PoliciesLodgingFoodingProfile : Profile
    {

        public PoliciesLodgingFoodingProfile()
        {
            CreateMap<PoliciesLodgingFooding, PoliciesLodgingFoodingDto>().ReverseMap();            
            CreateMap<AddPoliciesLodgingFoodingCommand, PoliciesLodgingFooding>();
            CreateMap<UpdatePoliciesLodgingFoodingCommand, PoliciesLodgingFooding>();
        }
    }
}
