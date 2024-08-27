using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class DeleteExpenseMiscCommand :IRequest<ServiceResponse<bool>>
    {
        public Guid? MasterExpenseId { get; set; }
    }
}
