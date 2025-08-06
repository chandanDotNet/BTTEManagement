using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.City.Commands;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CityProfile'
    public class CityProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CityProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CityProfile.CityProfile()'
        public CityProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CityProfile.CityProfile()'
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<AddCityCommand, City>();
            CreateMap<UpdateCityCommand, City>();
        }
    }
}
