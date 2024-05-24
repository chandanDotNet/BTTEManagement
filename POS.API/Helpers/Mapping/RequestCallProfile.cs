using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class RequestCallProfile :Profile
    {
        public RequestCallProfile()
        {
            CreateMap<RequestCall, RequestCallDto>().ReverseMap();
            CreateMap<AddRequestCallCommand, RequestCall>();
        }
    }
}
