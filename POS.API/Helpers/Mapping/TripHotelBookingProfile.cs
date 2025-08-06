using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripHotelBookingProfile'
    public class TripHotelBookingProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripHotelBookingProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripHotelBookingProfile.TripHotelBookingProfile()'
        public TripHotelBookingProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripHotelBookingProfile.TripHotelBookingProfile()'
        {
            CreateMap<TripHotelBooking, TripHotelBookingDto>().ReverseMap();

            CreateMap<AddTripHotelBookingCommand, TripHotelBooking>();
            //CreateMap<UpdateTripHotelBookingCommand, TripHotelBooking>();
        }

    }
}
