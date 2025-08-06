using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripTrackingProfile'
    public class TripTrackingProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripTrackingProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TripTrackingProfile.TripTrackingProfile()'
        public TripTrackingProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TripTrackingProfile.TripTrackingProfile()'
        {
            CreateMap<TripTracking, TripTrackingDto>().ReverseMap();
            CreateMap<AddTripTrackingCommand, TripTracking>();
            //CreateMap<UpdateTripCommand, Trip>();
        }
    }
}