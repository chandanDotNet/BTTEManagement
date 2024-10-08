﻿using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TripItinerary:BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string TripBy { get; set; }
        public string BookTypeBy { get; set; }
        public string TripWayType { get; set; }
        public Guid? DepartureCityId { get; set; }
        public City DepartureCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public Guid? ArrivalCityId { get; set; }
        public City ArrivalCity { get; set; }
        //public DateTime? ReturnDate { get; set; }
        private DateTime? _returnDate;
        public DateTime? ReturnDate
        {
            get => _returnDate;
            set => _returnDate = value;
        }
        public string? TripPreference1 { get; set; }
        public string? TripPreference2 { get; set; }
        public string? TripPreferenceTime { get; set; }
        public string? TripReturnPreferenceTime { get; set; }
        public string? TripPreferenceSeat { get; set; }

        public decimal? TentativeAmount { get; set; }
        public string? BookStatus { get; set; }
        public Guid? ExpenseId { get; set; }

        public string? ApprovalStatus { get; set; }
        public DateTime? ApprovalStatusDate { get; set; }
        public Guid? ApprovalStatusBy { get; set; }
       
        public decimal? Amount { get; set; }
        public string? TicketReceiptName { get; set; }
        public string? TicketReceiptPath { get; set; }
        public string? ApprovalDocumentsReceiptName { get; set; }
        public string? ApprovalDocumentsReceiptPath { get; set; }

        public string? DepartureCityName { get; set; }
        public string? ArrivalCityName { get; set; }
        public string? TrainClass { get; set; }
        public string? PreferredTrain { get; set; }
        public string? PickupTime { get; set; }
        public string? BusType { get; set; }
        public string? CarType { get; set; }
        public string? NoOfTickets { get; set; }
        public DateTime? RescheduleDepartureDate { get; set; }
        public string? RescheduleReason { get; set; }
        public bool? IsReschedule { get; set; }=false;
        public bool? IsQuotationUpload { get; set; } = false;
        public string? RMStatus { get; set; }
        public bool? IsCancel { get; set; } = false;
        public List<ItineraryTicketBooking> ItineraryTicketBooking { get; set; }
        public List<ItineraryTicketBookingQuotation> ItineraryTicketBookingQuotation { get; set; } = new List<ItineraryTicketBookingQuotation>();
        public List<CancelTripItineraryHotelUser> CancelTripItineraryHotelUser { get; set; } = new List<CancelTripItineraryHotelUser>();
    }
}
