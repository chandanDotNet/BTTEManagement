using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ItineraryHotelBookingQuotationProfile'
    public class ItineraryHotelBookingQuotationProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ItineraryHotelBookingQuotationProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ItineraryHotelBookingQuotationProfile.ItineraryHotelBookingQuotationProfile()'
        public ItineraryHotelBookingQuotationProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ItineraryHotelBookingQuotationProfile.ItineraryHotelBookingQuotationProfile()'
        {
            CreateMap<ItineraryHotelBookingQuotation, ItineraryHotelBookingQuotationDto>().ReverseMap();
            CreateMap<AddItineraryHotelBookingQuotationCommand, ItineraryHotelBookingQuotation>();
        }
    }
}
