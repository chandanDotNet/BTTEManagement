using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class RescheduleTripItineraryHotelCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public DateTime? RescheduleDepartureDate { get; set; }
        public string? RescheduleReason { get; set; }
        public bool? IsReschedule { get; set; }
        public bool? IsItinerary { get; set; }
    }
}
