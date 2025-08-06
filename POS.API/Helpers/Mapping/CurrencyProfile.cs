using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Currency.Commands;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CurrencyProfile'
    public class CurrencyProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CurrencyProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CurrencyProfile.CurrencyProfile()'
        public CurrencyProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CurrencyProfile.CurrencyProfile()'
        {
            CreateMap<CurrencyDto, Currency>().ReverseMap();
            CreateMap<AddCurrencyCommand, Currency>();
            CreateMap<UpdateCurrencyCommand, Currency>();
        }
    }
}
