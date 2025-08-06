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
        public DateTime? ItineraryRescheduleDepartureDate { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? RescheduleReason { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool? IsReschedule { get; set; }
        public bool? IsItinerary { get; set; }
        public DateTime? RescheduleCheckIn { get; set; }
        public DateTime? RescheduleCheckOut { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? RescheduleCheckInTime { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? RescheduleCheckOutTime { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool? IsRescheduleChargePlus { get; set; }
    }
}
