using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ExpenseTracking.Handler
{
    public class GetAllExpenseTrackingQueryHandler : IRequestHandler<GetAllExpenseTrackingQuery, ExpenseTrackingList>
    {
        private readonly IExpenseTrackingRepository _expenseTrackingRepository;
        public GetAllExpenseTrackingQueryHandler(IExpenseTrackingRepository expenseTrackingRepository)
        {
            _expenseTrackingRepository = expenseTrackingRepository;
        }
        public async Task<ExpenseTrackingList> Handle(GetAllExpenseTrackingQuery request, CancellationToken cancellationToken)
        {
            return await _expenseTrackingRepository.GetExpenseTrackings(request.ExpenseTrackingResource);
        }
    }
}
