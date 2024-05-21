using AutoMapper;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class UpdateExpenseAndMasterEpenseCommandHandler : IRequestHandler<UpdateExpenseAndMasterExpenseCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseAndMasterEpenseCommandHandler> _logger;

        public UpdateExpenseAndMasterEpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateExpenseAndMasterEpenseCommandHandler> logger)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseAndMasterExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseRepository.FindAsync(request.ExpenseId.Value);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }
            entityExist.AccountStatus = request.AccountStatus;
            entityExist.AccountStatusRemarks = request.AccountStatusRemarks;

            var masterEntityExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
            masterEntityExist.ReimbursementAmount = request.ReimbursementAmount + masterEntityExist.ReimbursementAmount;
            if (masterEntityExist.ReimbursementAmount == masterEntityExist.TotalAmount)
            {
                masterEntityExist.ReimbursementStatus = "FULL";
            }
            else if (masterEntityExist.ReimbursementAmount != 0)
            {
                masterEntityExist.ReimbursementStatus = "PARTIAL";
            }
            else if (masterEntityExist.ReimbursementAmount == 0)
            {
                masterEntityExist.ReimbursementStatus = "PENDING";
            }

            //_mapper.Map(request, entityExist);            

            _expenseRepository.Update(entityExist);
            _masterExpenseRepository.Update(masterEntityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
