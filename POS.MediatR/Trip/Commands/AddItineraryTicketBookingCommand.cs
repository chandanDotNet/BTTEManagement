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
    public class AddItineraryTicketBookingCommand : IRequest<ServiceResponse<ItineraryTicketBookingDto>>
    {

        
        public Guid TripItineraryId { get; set; }
        public decimal? BookingAmount { get; set; }
        public decimal? AgentCharge { get; set; }
        public decimal? CancelationCharge { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? CancelationReason { get; set; }
        public string? TicketReceiptName { get; set; }
        public string? TicketDocumentData { get; set; }
        public string? ApprovalDocumentsReceiptName { get; set; }
        public string? ApprovalDocumentData { get; set; }
        public string? Status { get; set; }
        public bool? IsAvail { get; set; }
        public Guid? ActionBy { get; set; }
        public string? VendorName { get; set; }
        public string? BookingDate { get; set; }
        public string? BookingTime { get; set; }
    }
}
