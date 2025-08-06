using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesSettingProfile'
    public class PoliciesSettingProfile:Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesSettingProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesSettingProfile.PoliciesSettingProfile()'
        public  PoliciesSettingProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesSettingProfile.PoliciesSettingProfile()'
        {
            CreateMap<PoliciesSetting, PoliciesSettingDto>().ReverseMap();
            CreateMap<AddPoliciesSettingCommand, PoliciesSetting>();
            CreateMap<UpdatePoliciesSettingCommand, PoliciesSetting>();

        }
    }
}
