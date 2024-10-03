using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class AllAccountUpdateExpenseAndMasterExpenseCommand
    {
        public List<UpdateExpenseAndMasterExpenseCommand> AllUpdateExpenseAndMasterExpense { get; set;}
    }
}
