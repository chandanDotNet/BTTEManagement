using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using BTTEM.Repository.Expense;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class GetAllCarBikeLogBookExpenseQueryHandler : IRequestHandler<GetAllCarBikeLogBookExpenseQuery, CarBikeLogBookExpenseList>
    {

        private readonly ICarBikeLogBookExpenseRepository _carBikeLogBookExpenseRepository;

        public GetAllCarBikeLogBookExpenseQueryHandler(
            ICarBikeLogBookExpenseRepository carBikeLogBookExpenseRepository)
        {
            _carBikeLogBookExpenseRepository = carBikeLogBookExpenseRepository;
        }

        public async Task<CarBikeLogBookExpenseList> Handle(GetAllCarBikeLogBookExpenseQuery request, CancellationToken cancellationToken)
        {
            return await _carBikeLogBookExpenseRepository.GetAllCarBikeLogBookExpense(request.ExpenseResource);
        }
    }
}
