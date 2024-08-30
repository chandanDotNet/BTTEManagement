using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class ItineraryHotelBookingQuotationProfile :Profile
    {
        public ItineraryHotelBookingQuotationProfile()
        {
            CreateMap<ItineraryHotelBookingQuotation, ItineraryHotelBookingQuotationDto>().ReverseMap();
            CreateMap<AddItineraryHotelBookingQuotationCommand, ItineraryHotelBookingQuotation>();
        }
    }
}
