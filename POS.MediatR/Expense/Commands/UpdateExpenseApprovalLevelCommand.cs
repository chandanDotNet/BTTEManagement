using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class UpdateExpenseApprovalLevelCommand :IRequest<ServiceResponse<bool>>
    {
        public Guid? MasterExpenseId { get; set; }
        public Guid? AccountsCheckerId { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountsCheckerStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public int? AccountsApprovalStage { get; set; }

    }
}
