using BTTEM.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class AllUpdateExpenseStatusCommand
    {

        public List<UpdateExpenseStatusCommand> updateExpenseStatus { get; set; }   
    }
}
