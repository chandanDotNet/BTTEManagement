using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
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
    public class UpdateMasterExpenseStatusCommandHandler : IRequestHandler<UpdateMasterExpenseStatusCommand, ServiceResponse<bool>>
    {
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly ILogger<UpdateMasterExpenseStatusCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        public UpdateMasterExpenseStatusCommandHandler(IMasterExpenseRepository masterExpenseRepository,
            ILogger<UpdateMasterExpenseStatusCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            IUserRepository userRepository,
            IWalletRepository walletRepository)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _logger = logger;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
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

                if(request.ApprovalStage == "APPROVED")
                {
                    //=================
                    AddWalletCommand requestWallet = new AddWalletCommand();
                    decimal amount = 0;
                    var appUser = await _userRepository.FindAsync(entityExist.CreatedBy);
                    if (appUser != null && appUser.IsPermanentAdvance == true)
                    {
                        amount = appUser.PermanentAdvance.Value;

                        requestWallet.CurrentWalletBalance = ((decimal)(amount - entityExist.PayableAmount));
                        requestWallet.UserId = entityExist.CreatedBy;
                        requestWallet.IsCredit = false;
                        requestWallet.PermanentAdvance = amount;
                        requestWallet.ExpenseAmount = (decimal)entityExist.PayableAmount;
                        //var entity = _mapper.Map<Wallet>(request);
                        //_walletRepository.Add(entity);
                        appUser.PermanentAdvance = requestWallet.CurrentWalletBalance;
                        _userRepository.Update(appUser);
                    }
                    else
                    {

                    }
                }
            }

            if (request.Status == "ROLLBACK" && entityExist.RollbackCount <= 3)
            {
                entityExist.RollbackCount = entityExist.RollbackCount + 1;
                entityExist.Status = "YET TO SUBMIT";
            }

            if (request.ApprovalStage == "REJECTED")
            {
                entityExist.RejectedReason = request.RejectedReason;
            }

            if (!string.IsNullOrEmpty(request.JourneyNumber))
            {
                entityExist.JourneyNumber = request.JourneyNumber;
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
