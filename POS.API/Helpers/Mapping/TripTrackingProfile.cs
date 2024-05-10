using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class TripTrackingProfile : Profile
    {
        public TripTrackingProfile()
        {
            CreateMap<TripTracking, TripTrackingDto>().ReverseMap();
            CreateMap<AddTripTrackingCommand, TripTracking>();
            //CreateMap<UpdateTripCommand, Trip>();
        }
    }
}