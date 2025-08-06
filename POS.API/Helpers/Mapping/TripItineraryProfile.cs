using AutoMapper;
using BTTEM.Data;

using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripItineraryProfile'
    public class TripItineraryProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripItineraryProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripItineraryProfile.TripItineraryProfile()'
        public TripItineraryProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripItineraryProfile.TripItineraryProfile()'
        {
            CreateMap<TripItinerary, TripItineraryDto>().ReverseMap();
           

            CreateMap<AddTripItineraryCommand, TripItinerary>();
            CreateMap<UpdateTripItineraryCommand, TripItinerary>();
            CreateMap<UpdateTripItineraryBookStatusCommand, TripItinerary>();

           
        }
    }
}
