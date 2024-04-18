using BTTEM.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class GetAllTripItineraryQuery : IRequest<List<TripItineraryDto>>
    {
        public Guid? Id { get; set; }
        //public Guid? TripId { get; set; }

    }
}
