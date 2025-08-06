using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Tax.Commands;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TaxProfile'
    public class TaxProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TaxProfile'
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaxProfile()
        {
            CreateMap<Tax, TaxDto>().ReverseMap();
            CreateMap<AddTaxCommand, Tax>();
            CreateMap<UpdateTaxCommand, Tax>();

            CreateMap<TaxCode, TaxCodeDto>().ReverseMap();
        }
    }
}
