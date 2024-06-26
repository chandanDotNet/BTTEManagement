﻿using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
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
        private readonly UserInfoToken _userInfoToken;
        public UpdateMasterExpenseStatusCommandHandler(IMasterExpenseRepository masterExpenseRepository,
            ILogger<UpdateMasterExpenseStatusCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _logger = logger;
            _uow = uow;
            _userInfoToken = userInfoToken;
        }
        public async Task<ServiceResponse<bool>> Handle(UpdateMasterExpenseStatusCommand request, CancellationToken cancellationToken)
        {
            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
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
                entityExist.ApprovalStageBy = LoginUserId;
                entityExist.ApprovalStageDate = DateTime.Now;
            }

            if (request.Status == "ROLLBACK" && entityExist.RollbackCount <= 3)
            {
                entityExist.RollbackCount = entityExist.RollbackCount + 1;
                entityExist.Status = "YET TO SUBMIT";
            }

            if (!string.IsNullOrEmpty(request.JourneyNumber))
            {
                entityExist.JourneyNumber = entityExist.JourneyNumber;
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
