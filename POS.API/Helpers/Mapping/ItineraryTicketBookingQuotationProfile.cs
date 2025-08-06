using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingQuotationProfile'
    public class ItineraryTicketBookingQuotationProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingQuotationProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingQuotationProfile.ItineraryTicketBookingQuotationProfile()'
        public ItineraryTicketBookingQuotationProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ItineraryTicketBookingQuotationProfile.ItineraryTicketBookingQuotationProfile()'
        {
            CreateMap<ItineraryTicketBookingQuotation, ItineraryTicketBookingQuotationDto>().ReverseMap();
            CreateMap<AddItineraryTicketBookingQuotationCommand, ItineraryTicketBookingQuotation>();
        }
    }
}
