﻿using BTTEM.Data.Dto;
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
        public Guid? TripId { get; set; }
        public string BookTypeBy { get; set; }
        public Guid? CityId { get; set; }
        public City City { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public decimal? TentativeAmount { get; set; }
        public string? BookStatus { get; set; }
        public Guid? ExpenseId { get; set; }

        public string? ApprovalStatus { get; set; }
        public DateTime? ApprovalStatusDate { get; set; }
        public Guid? ApprovalStatusStatusBy { get; set; }
        public decimal? Amount { get; set; }
        public string? BillReceiptName { get; set; }
        public string? BillReceiptPath { get; set; }
        public string? ApprovalDocumentsReceiptName { get; set; }
        public string? ApprovalDocumentsReceiptPath { get; set; }

        public string? CityName { get; set; }
        public string? NearbyLocation { get; set; }
        public string? PreferredHotel { get; set; }
        public decimal? CancelationCharge { get; set; } = 0;
        public string? CancelationReason { get; set; }
        public string? NoOfRoom { get; set; }
        public DateTime? CancelationDate { get; set; }
        public bool? IsReschedule { get; set; } = false;
        public string? RescheduleStatus { get; set; }
        public string? RescheduleReason { get; set; }
        public decimal? RescheduleCharge { get; set; } = 0;
        public DateTime? RescheduleCheckIn { get; set; }
        public DateTime? RescheduleCheckOut { get; set; }
        public string? RescheduleCheckInTime { get; set; }
        public string? RescheduleCheckOutTime { get; set; }
        public string? RescheduleBillReceiptName { get; set; }
        public string? RescheduleBillReceiptPath { get; set; }
        public string? RescheduleApprovalDocumentsReceiptName { get; set; }
        public string? RescheduleApprovalDocumentsReceiptPath { get; set; }
        public Guid? VendorId { get; set; }
        public VendorDto? Vendor { get; set; }
        public decimal? AgentCharge { get; set; }
        public decimal? BookingAmount { get; set; }
        public bool? IsQuotationUpload { get; set; }
        public string? RMStatus { get; set; }
        public bool? IsCancel { get; set; } = false;
        public string? BookingNumber { get; set; }
        public bool? IsRescheduleChargePlus { get; set; } = false;
        public decimal? TotalAmount { get; set; }
        public List<ItineraryHotelBookingQuotationDto> ItineraryHotelQuotationBookingDto { get; set; } = new List<ItineraryHotelBookingQuotationDto>();
        public List<CancelTripItineraryHotelUserDto> CancelTripItineraryHotelUserDto { get; set; } = new List<CancelTripItineraryHotelUserDto>();
    }
}
