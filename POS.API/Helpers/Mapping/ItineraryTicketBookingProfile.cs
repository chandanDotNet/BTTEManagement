using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingProfile'
    public class ItineraryTicketBookingProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingProfile.ItineraryTicketBookingProfile()'
        public ItineraryTicketBookingProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingProfile.ItineraryTicketBookingProfile()'
        {
            CreateMap<ItineraryTicketBooking, ItineraryTicketBookingDto>().ReverseMap();
            CreateMap<AddItineraryTicketBookingCommand, ItineraryTicketBooking>();
            CreateMap<UpdateItineraryTicketBookingIsAvailCommand, ItineraryTicketBooking>();
            CreateMap<UpdateItineraryTicketBookingCommand, ItineraryTicketBooking>();
        }
    }
}
