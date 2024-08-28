using BTTEM.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ItineraryTicketBookingDto
    {

        public Guid Id { get; set; }
        public Guid TripItineraryId { get; set; }
        public decimal? BookingAmount { get; set; }
        public decimal? AgentCharge { get; set; }
        public decimal? CancelationCharge { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? CancelationReason { get; set; }
        public string? TicketReceiptName { get; set; }
        public string? TicketReceiptPath { get; set; }
        public string? ApprovalDocumentsReceiptName { get; set; }
        public string? ApprovalDocumentsReceiptPath { get; set; }
        public string? Status { get; set; }
        public bool? IsAvail { get; set; }
        public Guid? ActionBy { get; set; }
        public Guid? VendorId { get; set; }
        public VendorDto? Vendor { get; set; }
        public string? VendorName { get; set; }
        public string? BookingDate { get; set; }
        public string? BookingTime { get; set; }
        public bool? IsReschedule { get; set; }
        public string? RescheduleStatus { get; set; }
        public decimal? RescheduleCharge { get; set; }
        public string? RescheduleBookingDate { get; set; }
        public string? RescheduleBookingTime { get; set; }
        public string? RescheduleTicketReceiptName { get; set; }
        public string? RescheduleTicketReceiptPath { get; set; }
        public string? RescheduleApprovalDocumentsReceiptName { get; set; }
        public string? RescheduleApprovalDocumentsReceiptPath { get; set; }
    }
}
