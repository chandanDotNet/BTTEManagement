using AutoMapper;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesLodgingFoodingProfile'
    public class PoliciesLodgingFoodingProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesLodgingFoodingProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesLodgingFoodingProfile.PoliciesLodgingFoodingProfile()'
        public PoliciesLodgingFoodingProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesLodgingFoodingProfile.PoliciesLodgingFoodingProfile()'
        {
            CreateMap<PoliciesLodgingFooding, PoliciesLodgingFoodingDto>().ReverseMap();            
            CreateMap<AddPoliciesLodgingFoodingCommand, PoliciesLodgingFooding>();
            CreateMap<UpdatePoliciesLodgingFoodingCommand, PoliciesLodgingFooding>();
        }
    }
}
