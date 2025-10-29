using BTTEM.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class GetMMTBookingRequestDetailsDataQuery : IRequest<MMTData>
    {
        public Guid TripId { get; set; }
    }
}