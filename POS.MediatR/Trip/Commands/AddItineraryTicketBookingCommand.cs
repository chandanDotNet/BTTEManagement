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
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? CancelationReason { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? TicketReceiptName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? TicketDocumentData { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ApprovalDocumentsReceiptName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ApprovalDocumentData { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Status { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool? IsAvail { get; set; }
        public Guid? ActionBy { get; set; }
        public Guid? VendorId { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? VendorName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? BookingDate { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? BookingTime { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string ServiceChargeNameOne { get; set; }
        public string ServiceChargeOne { get; set; }
        public string ServiceChargeNameTwo { get; set; }
        public string ServiceChargeTwo { get; set; }
        public string ServiceChargeNameThree { get; set; }
        public string ServiceChargeThree { get; set; }
        public string ServiceChargeNameFour { get; set; }
        public string ServiceChargeFour { get; set; }
        public string ServiceChargeNameFive { get; set; }
        public string ServiceChargeFive { get; set; }
        public string TaxAmountNameOne { get; set; }
        public string TaxAmountOne { get; set; }
        public string TaxAmountNameTwo { get; set; }
        public string TaxAmountTwo { get; set; }
        public string TaxAmountNameThree { get; set; }
        public string TaxAmountThree { get; set; }
        public string TaxAmountNameFour { get; set; }
        public string TaxAmountFour { get; set; }
        public string TaxAmountNameFive { get; set; }
        public string TaxAmountFive { get; set; }
        public string PNRNumber { get; set; }
        public bool IsServiceChargePlus { get; set; }
        public bool IsAmountConfirm { get; set; }
    }
}
