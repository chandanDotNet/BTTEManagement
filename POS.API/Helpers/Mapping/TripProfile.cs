using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class TripProfile :Profile
    {

        public TripProfile()
        {
            CreateMap<Trip, TripDto>().ReverseMap();

            CreateMap<AddTripCommand, Trip>();
            CreateMap<UpdateTripCommand, Trip>();
        }
    }
}
