using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class ItineraryTicketBookingQuotationProfile : Profile
    {
        public ItineraryTicketBookingQuotationProfile()
        {
            CreateMap<ItineraryTicketBookingQuotation, ItineraryTicketBookingQuotationDto>().ReverseMap();
            CreateMap<AddItineraryTicketBookingQuotationCommand, ItineraryTicketBookingQuotation>();
        }
    }
}
