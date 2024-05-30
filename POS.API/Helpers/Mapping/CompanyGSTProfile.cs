using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class CompanyGSTProfile : Profile
    {
        public CompanyGSTProfile()
        {
            CreateMap<CompanyGST, CompanyGSTDto>().ReverseMap();
            CreateMap<AddUpdateStateWiseGSTCommand, CompanyGST>();
        }
    }
}
