using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class TripItineraryProfile :Profile
    {

        public TripItineraryProfile()
        {
            CreateMap<TripItinerary, TripItineraryDto>().ReverseMap();

            CreateMap<AddTripItineraryCommand, TripItinerary>();
            CreateMap<UpdateTripItineraryCommand, TripItinerary>();
            CreateMap<UpdateTripItineraryBookStatusCommand, TripItinerary>();
        }
    }
}
