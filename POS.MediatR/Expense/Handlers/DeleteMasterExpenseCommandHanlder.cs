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

namespace BTTEM.MediatR.Handlers
{
    public class DeleteMasterExpenseCommandHanlder : IRequestHandler<DeleteMasterExpenseCommand, ServiceResponse<bool>>
    {

        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly ILogger<DeleteMasterExpenseCommandHanlder> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteMasterExpenseCommandHanlder(IMasterExpenseRepository masterExpenseRepository,
            ILogger<DeleteMasterExpenseCommandHanlder> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _logger = logger;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteMasterExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _masterExpenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            entityExist.IsDeleted = true;
            _masterExpenseRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith204();
        }

    }
}
