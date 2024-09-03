using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class CancelTripItineraryHotelUserDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? TripItineraryId { get; set; }
        public Guid? TripHotelBookingId { get; set; }
        public bool IsCancelRequest { get; set; }
        public bool? IsHotel { get; set; } = false;
    }
}
