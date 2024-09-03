using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using POS.Data;
using POS.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class CancelTripItineraryHotelUserProfile : Profile
    {
        public CancelTripItineraryHotelUserProfile()
        {
            CreateMap<CancelTripItineraryHotelUser, CancelTripItineraryHotelUserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
