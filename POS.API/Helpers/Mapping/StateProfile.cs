using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StateProfile'
    public class StateProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StateProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StateProfile.StateProfile()'
        public StateProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StateProfile.StateProfile()'
        {
            CreateMap<State,StateDto>().ReverseMap();
        }
    }
}
