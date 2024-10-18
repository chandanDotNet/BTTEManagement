using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
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
    public class UpdateExpenseAndMasterExpenseApprovalLevelCommandHandler : IRequestHandler<UpdateExpenseAndMasterExpenseApprovalLevelCommand, ServiceResponse<bool>>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseAndMasterEpenseCommandHandler> _logger;

        public UpdateExpenseAndMasterExpenseApprovalLevelCommandHandler(
            IExpenseRepository expenseRepository,
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateExpenseAndMasterEpenseCommandHandler> logger,
            IUserRepository userRepository,
            ITripRepository tripRepository)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _tripRepository = tripRepository;
        }
        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseAndMasterExpenseApprovalLevelCommand request, CancellationToken cancellationToken)
        {
            if (request.ExpenseId.HasValue)
            {
                var entityExist = await _expenseRepository.FindAsync(request.ExpenseId.Value);
                if (entityExist == null)
                {
                    _logger.LogError("Expense does not exists.");
                    return ServiceResponse<bool>.Return409("Expense does not exists.");
                }
                if (request.ExpenseApprovalStage == 3)
                {
                    entityExist.AccountStatus = request.AccountStatus;
                    entityExist.AccountStatusRemarks = request.AccountStatusRemarks;
                    entityExist.ReimbursementAmount = request.ReimbursementAmount;

                    var masterEntityExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
                    masterEntityExist.ReimbursementAmount = request.LevelReimbursementAmount;

                    masterEntityExist.ThirdLevelReimbursementAmount = masterEntityExist.ReimbursementAmount;


                    if (masterEntityExist.ReimbursementAmount == masterEntityExist.TotalAmount)
                    {
                        masterEntityExist.ReimbursementStatus = "FULL";
                        //masterEntityExist.Status = "Reimbursement";
                    }
                    else if (masterEntityExist.ReimbursementAmount != 0)
                    {
                        masterEntityExist.ReimbursementStatus = "PARTIAL";
                        //masterEntityExist.Status = "Reimbursement";
                    }
                    else if (masterEntityExist.ReimbursementAmount == 0)
                    {
                        masterEntityExist.ReimbursementStatus = "PENDING";
                    }

                    entityExist.ApprovedByThirdLevel = request.ApprovedBy;
                    entityExist.ReimbursementAmountThirdLevel = request.ReimbursementAmount;
                    entityExist.AccountStatusThirdLevel = request.AccountStatus;
                    entityExist.AccountStatusRemarksThirdLevel = request.AccountStatusRemarks;
                    entityExist.ExpenseApprovalStage = 3;
                    //_mapper.Map(request, entityExist);            

                    _expenseRepository.Update(entityExist);
                    _masterExpenseRepository.Update(masterEntityExist);
                }

                if (request.ExpenseApprovalStage == 2)
                {
                    entityExist.ApprovedBySecondLevel = request.ApprovedBy;
                    entityExist.ReimbursementAmountSecondLevel = request.ReimbursementAmount;
                    entityExist.AccountStatusSecondLevel = request.AccountStatus;
                    entityExist.AccountStatusRemarksSecondLevel = request.AccountStatusRemarks;
                    entityExist.ExpenseApprovalStage = 2;
                    _expenseRepository.Update(entityExist);

                    var masterEntityExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
                    masterEntityExist.SecondLevelReimbursementAmount = request.LevelReimbursementAmount;
                    _masterExpenseRepository.Update(masterEntityExist);
                }

                if (request.ExpenseApprovalStage == 1)
                {
                    entityExist.ApprovedByFirstLevel = request.ApprovedBy;
                    entityExist.ReimbursementAmountFirstLevel = request.ReimbursementAmount;
                    entityExist.AccountStatusFirstLevel = request.AccountStatus;
                    entityExist.AccountStatusRemarksFirstLevel = request.AccountStatusRemarks;
                    entityExist.ExpenseApprovalStage = 1;
                    _expenseRepository.Update(entityExist);

                    var masterEntityExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);                    
                    masterEntityExist.FirstLevelReimbursementAmount = request.LevelReimbursementAmount;
                    _masterExpenseRepository.Update(masterEntityExist);
                }
            }

            if (request.MasterExpenseId.HasValue)
            {
                var masterEntityExist = await _masterExpenseRepository.FindAsync(request.MasterExpenseId.Value);

                if (request.ExpenseApprovalStage == 3)
                {
                    //masterEntityExist.ReimbursementAmount = request.ReimbursementAmount;
                    masterEntityExist.IsExpenseCompleted = true;

                    if (masterEntityExist.ReimbursementAmount == masterEntityExist.TotalAmount)
                    {
                        masterEntityExist.ReimbursementStatus = "FULL";
                        masterEntityExist.Status = "REIMBURSED";
                    }
                    else if (masterEntityExist.ReimbursementAmount != 0)
                    {
                        masterEntityExist.ReimbursementStatus = "PARTIAL";
                        masterEntityExist.Status = "REIMBURSED";
                    }
                    else if (masterEntityExist.ReimbursementAmount == 0)
                    {
                        masterEntityExist.ReimbursementStatus = "PENDING";
                    }
                    if (!string.IsNullOrEmpty(request.ReimbursementRemarks))
                    {
                        masterEntityExist.ReimbursementRemarks = request.ReimbursementRemarks;
                    }

                    _masterExpenseRepository.Update(masterEntityExist);

                    if (masterEntityExist.TripId.HasValue)
                    {
                        var tripEntityExist = await _tripRepository.FindAsync(masterEntityExist.TripId.Value);
                        tripEntityExist.IsTripCompleted = true;
                        tripEntityExist.Status = "COMPLETED";
                        _tripRepository.Update(tripEntityExist);
                    }
                    AddWalletCommand requestWallet = new AddWalletCommand();
                    decimal amount = 0;
                    var appUser = await _userRepository.FindAsync(masterEntityExist.CreatedBy);
                    if (appUser != null && appUser.IsPermanentAdvance == true)
                    {
                        amount = appUser.PermanentAdvance.Value;
                        requestWallet.CurrentWalletBalance = ((decimal)(amount + masterEntityExist.PayableAmount));
                        requestWallet.UserId = masterEntityExist.CreatedBy;
                        requestWallet.IsCredit = false;
                        requestWallet.PermanentAdvance = amount;
                        requestWallet.ExpenseAmount = request.ReimbursementAmount;
                        //var entity = _mapper.Map<Wallet>(request);
                        //_walletRepository.Add(entity);
                        appUser.PermanentAdvance = requestWallet.CurrentWalletBalance;
                        _userRepository.Update(appUser);
                    }
                }

                if (request.ExpenseApprovalStage == 1)
                {
                    masterEntityExist.AccountsCheckerOneId = request.ApprovedBy;
                    masterEntityExist.AccountsCheckerOneStatus = request.checkApproval;
                    masterEntityExist.AccountsApprovalStage = request.ExpenseApprovalStage;
                    _masterExpenseRepository.Update(masterEntityExist);
                }

                else if (request.ExpenseApprovalStage == 2)
                {
                    masterEntityExist.AccountsCheckerTwoId = request.ApprovedBy;
                    masterEntityExist.AccountsCheckerTwoStatus = request.checkApproval;
                    masterEntityExist.AccountsApprovalStage = request.ExpenseApprovalStage;
                    _masterExpenseRepository.Update(masterEntityExist);
                }

                else if (request.ExpenseApprovalStage == 3)
                {
                    masterEntityExist.AccountsCheckerThreeId = request.ApprovedBy;
                    masterEntityExist.AccountsCheckerThreeStatus = request.checkApproval;
                    masterEntityExist.AccountsApprovalStage = request.ExpenseApprovalStage;
                    _masterExpenseRepository.Update(masterEntityExist);
                }

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
