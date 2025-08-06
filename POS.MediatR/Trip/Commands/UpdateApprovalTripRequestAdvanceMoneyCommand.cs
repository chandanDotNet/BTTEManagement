using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateApprovalTripRequestAdvanceMoneyCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public decimal? AdvanceMoneyApprovedAmount { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AdvanceMoneyRemarks { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string RequestAdvanceMoneyStatus { get; set; }
        public Guid? StatusUpdatedBy { get; set; }
        public DateTime? AdvanceAccountApprovedOn { get; set; }
        public Guid? AdvanceAccountApprovedBy { get; set; }
        public decimal? AdvanceAccountApprovedAmount { get; set; }
        public string AdvanceAccountApprovedStatus { get; set; }

    }
}