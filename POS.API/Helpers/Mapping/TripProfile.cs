using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class TripProfile :Profile
    {

        public TripProfile()
        {
            CreateMap<Trip, TripDto>().ReverseMap();
            CreateMap<GroupTrip, GroupTripDto>().ReverseMap();

            CreateMap<AddTripCommand, Trip>();
            CreateMap<UpdateTripCommand, Trip>().ReverseMap();
        }
    }
}
