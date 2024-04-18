using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TripHotelBookingDto
    {

        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string BookTypeBy { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
    }
}
