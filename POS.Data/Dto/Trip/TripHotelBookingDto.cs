﻿using POS.Data;
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
    }
}
