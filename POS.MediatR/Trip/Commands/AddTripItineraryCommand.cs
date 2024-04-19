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
    public class AddTripItineraryCommand : IRequest<ServiceResponse<TripItineraryDto>>
    {

        public List<TripItineraryDto> TripItinerary { get; set; }
        //public Guid TripId { get; set; }
        //public string TripBy { get; set; }
        //public string BookTypeBy { get; set; }
        //public string TripWayType { get; set; }
        //public Guid DepartureCityId { get; set; }
        //public DateTime DepartureDate { get; set; }
        //public Guid ArrivalCityId { get; set; }
        //public DateTime? ReturnDate { get; set; }
        //public string? TripPreference1 { get; set; }
        //public string? TripPreference2 { get; set; }
        //public string? TripPreferenceTime { get; set; }
        //public string? TripReturnPreferenceTime { get; set; }
        //public string? TripPreferenceSeat { get; set; }
    }
}
