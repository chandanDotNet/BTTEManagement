using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class TripHotelBookingProfile :Profile
    {

        public TripHotelBookingProfile()
        {
            CreateMap<TripHotelBooking, TripHotelBookingDto>().ReverseMap();

            CreateMap<AddTripHotelBookingCommand, TripHotelBooking>();
            //CreateMap<UpdateTripHotelBookingCommand, TripHotelBooking>();
        }

    }
}
