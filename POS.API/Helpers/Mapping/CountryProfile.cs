using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Country.Commands;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CountryProfile'
    public class CountryProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CountryProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CountryProfile.CountryProfile()'
        public CountryProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CountryProfile.CountryProfile()'
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<AddCountryCommand, Country>();
            CreateMap<UpdateCountryCommand, Country>();
        }
    }
}
