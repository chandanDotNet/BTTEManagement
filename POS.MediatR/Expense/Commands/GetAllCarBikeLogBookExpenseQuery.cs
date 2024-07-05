using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllCarBikeLogBookExpenseQuery : IRequest<CarBikeLogBookExpenseList>
    {
        public CarBikeLogBookExpenseResource ExpenseResource { get; set; }

    }
}
