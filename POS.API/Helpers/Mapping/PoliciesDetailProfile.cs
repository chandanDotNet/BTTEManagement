using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Product.Command;
using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesDetailProfile'
    public class PoliciesDetailProfile:Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesDetailProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PoliciesDetailProfile.PoliciesDetailProfile()'
        public PoliciesDetailProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PoliciesDetailProfile.PoliciesDetailProfile()'
        {
            CreateMap<PoliciesDetail, PoliciesDetailDto>().ReverseMap();
            //CreateMap<ProductTax, ProductTaxDto>().ReverseMap();
            CreateMap<AddPoliciesDetailCommand, PoliciesDetail>();
            CreateMap<UpdatePoliciesDetailCommand, PoliciesDetail>();
        }
    }
}
