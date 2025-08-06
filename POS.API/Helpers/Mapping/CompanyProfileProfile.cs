using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompanyProfileProfile'
    public class CompanyProfileProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompanyProfileProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CompanyProfileProfile.CompanyProfileProfile()'
        public CompanyProfileProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CompanyProfileProfile.CompanyProfileProfile()'
        {
            CreateMap<CompanyProfile, CompanyProfileDto>().ReverseMap();
            CreateMap<CompanyAccount, CompanyAccountDto>().ReverseMap();
            CreateMap<UpdateCompanyProfileCommand, CompanyProfile>();
        }
    }
}
