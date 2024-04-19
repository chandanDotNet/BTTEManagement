using BTTEM.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class GetAllTripHotelBookingQuery : IRequest<List<TripHotelBookingDto>>
    {
        public Guid? Id { get; set; }

    }
}
