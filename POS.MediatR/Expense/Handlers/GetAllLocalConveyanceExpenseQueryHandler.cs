using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using BTTEM.Repository.Expense;
using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class GetAllLocalConveyanceExpenseQueryHandler : IRequestHandler<GetAllLocalConveyanceExpenseQuery, LocalConveyanceExpenseList>
    {
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;

        public GetAllLocalConveyanceExpenseQueryHandler(
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository)
        {
            _localConveyanceExpenseRepository = localConveyanceExpenseRepository;
        }

        public async Task<LocalConveyanceExpenseList> Handle(GetAllLocalConveyanceExpenseQuery request, CancellationToken cancellationToken)
        {
            return await _localConveyanceExpenseRepository.GetAllLocalConveyanceExpense(request.ExpenseResource);
        }

    }
}
