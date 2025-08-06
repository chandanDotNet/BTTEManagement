using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripProfile'
    public class TripProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripProfile.TripProfile()'
        public TripProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripProfile.TripProfile()'
        {
            CreateMap<Trip, TripDto>().ReverseMap();
            CreateMap<GroupTrip, GroupTripDto>().ReverseMap();

            CreateMap<AddTripCommand, Trip>();
            CreateMap<UpdateTripCommand, Trip>().ReverseMap();
        }
    }
}
