using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.Repository;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace POS.MediatR.Handlers
{
    public class DeleteExpenseCommandHanlder
        : IRequestHandler<DeleteExpenseCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly ILogger<DeleteExpenseCommandHanlder> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteExpenseCommandHanlder(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository,
            ILogger<DeleteExpenseCommandHanlder> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _logger = logger;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            entityExist.IsDeleted = true;
            _expenseRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }

            var entityMasterExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
            if (entityMasterExist != null)
            {
                entityMasterExist.TotalAmount = (entityMasterExist.TotalAmount - entityExist.Amount);

                entityMasterExist.NoOfBill = entityMasterExist.NoOfBill - 1;

                _masterExpenseRepository.Update(entityMasterExist);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while saving Master Expense.");
                    return ServiceResponse<bool>.Return500();
                }
            }

            return ServiceResponse<bool>.ReturnResultWith204();
        }
    }
}
