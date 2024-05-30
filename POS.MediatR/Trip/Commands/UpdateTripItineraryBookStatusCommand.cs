using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateTripItineraryBookStatusCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public Guid? TripId { get; set; }
        public string? BookStatus { get; set; }
        public Guid? ExpenseId { get; set; }
        public bool? IsItinerary { get; set; }

        public string? ApprovalStatus { get; set; }
        public DateTime? ApprovalStatusDate { get; set; } = null;
        public Guid? ApprovalStatusBy { get; set; }
        public decimal? Amount { get; set; }
        public string? TicketReceiptName { get; set; }
        public string? TicketDocumentData { get; set; }
        public string? ApprovalDocumentsReceiptName { get; set; }
        public string? ApprovalDocumentData { get; set; }

    }
}
