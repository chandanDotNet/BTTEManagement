using BTTEM.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class AddExpenseDetailListCommand
    {
        public List<AddExpenseDetailCommand> AddExpenseDetailList { get; set; }
    }
}
