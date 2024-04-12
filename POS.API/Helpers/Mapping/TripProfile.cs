using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class TripProfile :Profile
    {

        public TripProfile()
        {
            CreateMap<Trip, TripDto>().ReverseMap();

            CreateMap<AddTripCommand, Trip>();
        }
    }
}
