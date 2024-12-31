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
        public string? AdvanceMoneyRemarks { get; set; }
        public string RequestAdvanceMoneyStatus { get; set; }
        public Guid? StatusUpdatedBy { get; set; }
        public DateTime? AdvanceAccountApprovedOn { get; set; }
        public Guid? AdvanceAccountApprovedBy { get; set; }
        public decimal? AdvanceAccountApprovedAmount { get; set; }
        public string AdvanceAccountApprovedStatus { get; set; }

    }
}