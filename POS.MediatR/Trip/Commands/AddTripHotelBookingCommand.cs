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
    public class AddTripHotelBookingCommand : IRequest<ServiceResponse<TripHotelBookingDto>>
    {

        public List<TripHotelBookingDto> tripHotelBooking { get; set; }
        //public Guid TripId { get; set; }
        //public string BookTypeBy { get; set; }
        //public Guid CityId { get; set; }
        //public DateTime? CheckIn { get; set; }
        //public DateTime? CheckOut { get; set; }
        //public string? CheckInTime { get; set; }
        //public string? CheckOutTime { get; set; }
    }
}
