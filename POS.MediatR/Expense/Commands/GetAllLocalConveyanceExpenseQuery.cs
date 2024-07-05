using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using MediatR;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllLocalConveyanceExpenseQuery : IRequest<LocalConveyanceExpenseList>
    {

        public LocalConveyanceExpenseResource ExpenseResource { get; set; }
    }
}
