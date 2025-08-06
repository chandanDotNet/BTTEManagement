using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using POS.Data;
using POS.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CancelTripItineraryHotelUserProfile'
    public class CancelTripItineraryHotelUserProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CancelTripItineraryHotelUserProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CancelTripItineraryHotelUserProfile.CancelTripItineraryHotelUserProfile()'
        public CancelTripItineraryHotelUserProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CancelTripItineraryHotelUserProfile.CancelTripItineraryHotelUserProfile()'
        {
            CreateMap<CancelTripItineraryHotelUser, CancelTripItineraryHotelUserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
