using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class StateProfile :Profile
    {
        public StateProfile()
        {
            CreateMap<State,StateDto>().ReverseMap();
        }
    }
}
