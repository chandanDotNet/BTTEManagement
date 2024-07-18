using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BTTEM.MediatR.Handlers
{
    public class DeleteExpenseByDateCommandHandler : IRequestHandler<DeleteExpenseByDateCommand, ServiceResponse<bool>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly ILogger<DeleteExpenseByDateCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteExpenseByDateCommandHandler(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository,
            ILogger<DeleteExpenseByDateCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _logger = logger;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteExpenseByDateCommand request, CancellationToken cancellationToken)
        {
            var entityExist = _expenseRepository.All.Where(a => a.MasterExpenseId == request.Id && a.ExpenseDate.Date== request.ExpenseDate.Date);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            //entityExist.IsDeleted = true;
            //entityExist.Amount = 0;
            _expenseRepository.RemoveRange(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }

            var entityMasterExist = await _masterExpenseRepository.FindAsync(request.Id);
            if (entityMasterExist != null)
            {
                var expList = _expenseRepository.All.Where(a => a.MasterExpenseId == request.Id && a.Amount>0);
                entityMasterExist.TotalAmount = expList.Sum(a => a.Amount);
                entityMasterExist.PayableAmount = expList.Sum(a => a.PayableAmount);

                entityMasterExist.NoOfBill = expList.Count();

                _masterExpenseRepository.Update(entityMasterExist);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while saving Master Expense.");
                    return ServiceResponse<bool>.Return500();
                }
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
