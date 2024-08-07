using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class UpdateExpenseDetailListCommand
    {

        public List<UpdateExpenseDetailCommand> UpdateExpenseDetailList {  get; set; }
    }
}
