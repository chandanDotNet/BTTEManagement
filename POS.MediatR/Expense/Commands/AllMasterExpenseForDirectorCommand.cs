using BTTEM.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class AllMasterExpenseForDirectorCommand
    {

        public Guid MasterExpenseId { get; set; }
        public bool IsDeviation { get; set; }
        public List<UpdateExpenseStatusCommand> ExpenseDetailsList { get; set; }
    }
}
