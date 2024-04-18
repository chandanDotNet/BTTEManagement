using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using BTTEM.Repository.Expense;
using MediatR;
using POS.MediatR.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class GetAllMasterExpenseQueryHandler : IRequestHandler<GetAllMasterExpenseQuery, MasterExpenseList>
    {
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IMapper _mapper;

        public GetAllMasterExpenseQueryHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _mapper = mapper;
        }

        public async Task<MasterExpenseList> Handle(GetAllMasterExpenseQuery request, CancellationToken cancellationToken)
        {
            return await _masterExpenseRepository.GetAllExpenses(request.ExpenseResource);
        }

    }
}
