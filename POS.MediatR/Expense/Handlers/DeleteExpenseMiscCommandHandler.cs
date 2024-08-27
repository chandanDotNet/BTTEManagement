using AutoMapper;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using BTTEM.Repository.Expense;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DeleteExpenseMiscCommandHandler : IRequestHandler<DeleteExpenseMiscCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseDetailRepository _expenseDetailRepository;
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteExpenseMiscCommandHandler> _logger;
        private readonly IMapper _mapper;
        public DeleteExpenseMiscCommandHandler(IExpenseRepository expenseRepository,
            IExpenseDetailRepository expenseDetailRepository,
            IExpenseDocumentRepository expenseDocumentRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteExpenseMiscCommandHandler> logger,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _expenseDetailRepository = expenseDetailRepository;
            _expenseDocumentRepository = expenseDocumentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteExpenseMiscCommand request, CancellationToken cancellationToken)
        {
            var entityExpense = await _expenseRepository.All.Where(x => x.MasterExpenseId == request.MasterExpenseId).ToListAsync();
            if (entityExpense != null)
            {
                _expenseRepository.RemoveRange(entityExpense);
            }
            var entityExpenseDetail = await _expenseDetailRepository.All.Where(x => x.MasterExpenseId == request.MasterExpenseId).ToListAsync();
            if (entityExpenseDetail != null)
            {
                _expenseDetailRepository.RemoveRange(entityExpenseDetail);
            }
            var entityExpenseDocument = await _expenseDocumentRepository.All.Where(x => entityExpense.Select(e => e.Id).Contains(x.ExpenseId.Value)).ToListAsync();
            if (entityExpenseDocument != null)
            {
                _expenseDocumentRepository.RemoveRange(entityExpenseDocument);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
