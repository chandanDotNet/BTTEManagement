using BTTEM.Repository.Expense;
using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllMasterExpenseQuery : IRequest<MasterExpenseList>
    {
        public ExpenseResource ExpenseResource { get; set; }

    }
}
