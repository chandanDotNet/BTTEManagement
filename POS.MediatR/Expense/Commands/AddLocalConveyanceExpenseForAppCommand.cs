using BTTEM.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class AddLocalConveyanceExpenseForAppCommand
    {

        public List<AddLocalConveyanceExpenseCommand> addLocalConveyanceExpenseData {  get; set; }   
    }
}
