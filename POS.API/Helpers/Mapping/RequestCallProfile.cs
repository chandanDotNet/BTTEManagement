using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RequestCallProfile'
    public class RequestCallProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RequestCallProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RequestCallProfile.RequestCallProfile()'
        public RequestCallProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RequestCallProfile.RequestCallProfile()'
        {
            CreateMap<RequestCall, RequestCallDto>().ReverseMap();
            CreateMap<AddRequestCallCommand, RequestCall>();
        }
    }
}
