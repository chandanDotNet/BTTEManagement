using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
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
    public class UpdateExpenseApprovalLevelCommandHandler : IRequestHandler<UpdateExpenseApprovalLevelCommand, ServiceResponse<bool>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddMasterExpenseCommandHandler> _logger;
        private IMediator _mediator;
        public UpdateExpenseApprovalLevelCommandHandler(IUserRoleRepository userRoleRepository, UserInfoToken userInfoToken, IMasterExpenseRepository masterExpenseRepository, IUnitOfWork<POSDbContext> uow, IMapper mapper, ILogger<AddMasterExpenseCommandHandler> logger, IMediator mediator)
        {
            _userRoleRepository = userRoleRepository;
            _userInfoToken = userInfoToken;
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseApprovalLevelCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _masterExpenseRepository.FindAsync(request.MasterExpenseId.Value);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }
            if (request.AccountsApprovalStage == 1)
            {
                entityExist.AccountsCheckerOneId = request.AccountsCheckerId;
                entityExist.AccountsCheckerOneStatus = request.AccountsCheckerStatus;
                entityExist.AccountsApprovalStage = request.AccountsApprovalStage;

            }
            else if (request.AccountsApprovalStage == 2)
            {
                entityExist.AccountsCheckerTwoId = request.AccountsCheckerId;
                entityExist.AccountsCheckerTwoStatus = request.AccountsCheckerStatus;
                entityExist.AccountsApprovalStage = request.AccountsApprovalStage;
            }
            else if (request.AccountsApprovalStage == 3)
            {
                entityExist.AccountsCheckerThreeId = request.AccountsCheckerId;
                entityExist.AccountsCheckerThreeStatus = request.AccountsCheckerStatus;
                entityExist.AccountsApprovalStage = request.AccountsApprovalStage;
            }
            
            _masterExpenseRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
