using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;

namespace BTTEM.API.Helpers.Mapping
{
    public class CancelTripItineraryHotelUserProfile : Profile
    {
        public CancelTripItineraryHotelUserProfile()
        {
            CreateMap<CancelTripItineraryHotelUser, CancelTripItineraryHotelUserDto>().ReverseMap();
        }
    }
}
