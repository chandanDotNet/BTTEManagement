﻿using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Product.Command;
using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class PoliciesDetailProfile:Profile
    {

        public PoliciesDetailProfile()
        {
            CreateMap<PoliciesDetail, PoliciesDetailDto>().ReverseMap();
            //CreateMap<ProductTax, ProductTaxDto>().ReverseMap();
            CreateMap<AddPoliciesDetailCommand, PoliciesDetail>();
            CreateMap<UpdatePoliciesDetailCommand, PoliciesDetail>();
        }
    }
}
