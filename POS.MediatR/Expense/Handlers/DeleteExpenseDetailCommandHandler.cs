using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DeleteExpenseDetailCommandHandler : IRequestHandler<DeleteExpenseDetailCommand, ServiceResponse<bool>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IExpenseDetailRepository _expenseDetailRepository;
        private readonly ILogger<DeleteExpenseDetailCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteExpenseDetailCommandHandler(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository, IExpenseDetailRepository expenseDetailRepository,
            ILogger<DeleteExpenseDetailCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _expenseDetailRepository = expenseDetailRepository;
            _logger = logger;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteExpenseDetailCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                var entityExist = _expenseDetailRepository.FindBy(a => a.Id == request.Id).FirstOrDefault();
                if (entityExist == null)
                {
                    return ServiceResponse<bool>.ReturnResultWith200(true);
                }
                _expenseDetailRepository.Remove(entityExist);
            }
            if (request.ExpenseId.HasValue)
            {
                var entityExist = await _expenseDetailRepository.FindBy(a => a.ExpenseId == request.ExpenseId).ToListAsync();
                if (entityExist.Count() == 0)
                {
                    return ServiceResponse<bool>.ReturnResultWith200(true);
                }
                _expenseDetailRepository.RemoveRange(entityExist);
            }
            if (request.MasterExpenseId.HasValue)
            {
                var entityExist = await _expenseDetailRepository.FindBy(a => a.MasterExpenseId == request.MasterExpenseId).ToListAsync();
                if (entityExist.Count() == 0)
                {
                    return ServiceResponse<bool>.ReturnResultWith200(true);
                }
                _expenseDetailRepository.RemoveRange(entityExist);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
