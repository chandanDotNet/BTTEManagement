using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class ItineraryTicketBookingProfile : Profile
    {
        public ItineraryTicketBookingProfile()
        {
            CreateMap<ItineraryTicketBooking, ItineraryTicketBookingDto>().ReverseMap();
            CreateMap<AddItineraryTicketBookingCommand, ItineraryTicketBooking>();
            CreateMap<UpdateItineraryTicketBookingIsAvailCommand, ItineraryTicketBooking>();
            CreateMap<UpdateItineraryTicketBookingCommand, ItineraryTicketBooking>();
        }
    }
}
