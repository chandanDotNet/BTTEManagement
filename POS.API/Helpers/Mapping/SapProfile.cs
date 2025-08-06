using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
    public class SapProfile : Profile
    {
        public SapProfile()
        {
            CreateMap<Sap, SapDto>().ReverseMap();
            CreateMap<AddSapCommand, Sap>();
        }
    }
}
