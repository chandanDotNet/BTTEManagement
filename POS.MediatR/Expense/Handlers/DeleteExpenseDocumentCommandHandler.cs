using BTTEM.MediatR.Expense.Commands;
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

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DeleteExpenseDocumentCommandHandler : IRequestHandler<DeleteExpenseDocumentCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly ILogger<DeleteExpenseDocumentCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteExpenseDocumentCommandHandler(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository, IExpenseDocumentRepository expenseDocumentRepository,
            ILogger<DeleteExpenseDocumentCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _expenseDocumentRepository = expenseDocumentRepository;
            _logger = logger;
            _uow = uow;
        }


        public async Task<ServiceResponse<bool>> Handle(DeleteExpenseDocumentCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseDocumentRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            _expenseDocumentRepository.Remove(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }           

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
