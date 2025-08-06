using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompanyGSTProfile'
    public class CompanyGSTProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompanyGSTProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompanyGSTProfile.CompanyGSTProfile()'
        public CompanyGSTProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompanyGSTProfile.CompanyGSTProfile()'
        {
            CreateMap<CompanyGST, CompanyGSTDto>().ReverseMap();
            CreateMap<AddUpdateStateWiseGSTCommand, CompanyGST>();
        }
    }
}
