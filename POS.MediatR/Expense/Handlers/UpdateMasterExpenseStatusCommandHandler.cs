﻿using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers 
{
    public class UpdateMasterExpenseStatusCommandHandler : IRequestHandler<UpdateMasterExpenseStatusCommand, ServiceResponse<bool>>
    {
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly ILogger<UpdateMasterExpenseStatusCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public UpdateMasterExpenseStatusCommandHandler(IMasterExpenseRepository masterExpenseRepository,
            ILogger<UpdateMasterExpenseStatusCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _logger = logger;
            _uow = uow;
        }
        public async Task<ServiceResponse<bool>> Handle(UpdateMasterExpenseStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _masterExpenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            if(!string.IsNullOrEmpty(request.Status))
            {
                entityExist.Status = request.Status;
            }
            if (!string.IsNullOrEmpty(request.ApprovalStage))
            {
                entityExist.ApprovalStage = request.ApprovalStage;
            }                     

            _masterExpenseRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
