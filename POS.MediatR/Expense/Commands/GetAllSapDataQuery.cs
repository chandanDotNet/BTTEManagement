using BTTEM.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class GetAllSapDataQuery : IRequest<Record>
    {
        public Guid MasterExpenseId { get; set; }
    }
}
