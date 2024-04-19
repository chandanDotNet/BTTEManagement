using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateTripItineraryCommand : IRequest<ServiceResponse<bool>>
    {

        public List<TripItineraryDto> TripItinerary { get; set; }
    }
}
