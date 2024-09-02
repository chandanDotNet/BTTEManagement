using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateItineraryTicketBookingCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public decimal? BookingAmount { get; set; }
        public decimal? AgentCharge { get; set; }
        public decimal? CancelationCharge { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? CancelationReason { get; set; }
        public string? Status { get; set; }
        public bool? IsAvail { get; set; }
        public Guid? ActionBy { get; set; }
        public Guid? VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? BookingDate { get; set; }
        public string? BookingTime { get; set; }
        public bool? IsReschedule { get; set; }
        public string? RescheduleStatus { get; set; }
        public string? RescheduleReason { get; set; }
        public decimal? RescheduleCharge { get; set; }
        public string? RescheduleBookingDate { get; set; }
        public string? RescheduleBookingTime { get; set; }
        public string? RescheduleTicketReceiptName { get; set; }
        public string? RescheduleTicketDocumentData { get; set; }
        public string? RescheduleApprovalDocumentsReceiptName { get; set; }
        public string? RescheduleApprovalDocumentData { get; set; }
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
    }
}
