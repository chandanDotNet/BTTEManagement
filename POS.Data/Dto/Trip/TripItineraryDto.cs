using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TripItineraryDto
    {

        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string TripBy { get; set; }
        public string BookTypeBy { get; set; }
        public string TripWayType { get; set; }
        public Guid DepartureCityId { get; set; }
        public DateTime DepartureDate { get; set; }
        public Guid ArrivalCityId { get; set; }
        private DateTime? _returnDate;
        public DateTime? ReturnDate
        {
            get => _returnDate;
            set => _returnDate = value;
        }
        //public DateTime? ReturnDate { get; set; }
        public string? TripPreference1 { get; set; }
        public string? TripPreference2 { get; set; }
        public string? TripPreferenceTime { get; set; }
        public string? TripReturnPreferenceTime { get; set; }
        public string? TripPreferenceSeat { get; set; }
        public decimal? TentativeAmount { get; set; }
        public CityDto DepartureCity { get; set; }
        public CityDto ArrivalCity { get; set; }
        public string? BookStatus { get; set; }
        public Guid? ExpenseId { get; set; }

    }
}
