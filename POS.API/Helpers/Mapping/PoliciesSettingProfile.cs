using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class PoliciesSettingProfile:Profile
    {

        public  PoliciesSettingProfile()
        {
            CreateMap<PoliciesSetting, PoliciesSettingDto>().ReverseMap();
            CreateMap<AddPoliciesSettingCommand, PoliciesSetting>();
            //CreateMap<UpdatePoliciesLodgingFoodingCommand, PoliciesLodgingFooding>();

        }
    }
}
