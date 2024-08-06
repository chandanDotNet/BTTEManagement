using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class UpdateLocalConveyanceExpenseForAppCommand
    {
        public List<UpdateLocalConveyanceExpenseCommand> updateLocalConveyanceExpenseData { get; set; }
    }
}
