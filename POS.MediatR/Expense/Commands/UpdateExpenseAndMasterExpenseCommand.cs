using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class UpdateExpenseAndMasterExpenseCommand : IRequest<ServiceResponse<bool>>
    {        
        public Guid? ExpenseId { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public decimal ReimbursementAmount { get; set; }
        public string AccountStatus { get; set; }
        public string AccountStatusRemarks { get; set; }
        public string ReimbursementStatus { get; set; }

    }
}
