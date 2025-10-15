using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class UpdateMasterExpenseStatusCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public string Status { get; set; }
        public string ApprovalStage { get; set; }
        public Guid? StatusBy { get; set; }
        public string JourneyNumber { get; set; }
        public string RejectedReason { get; set; }
    }
}
