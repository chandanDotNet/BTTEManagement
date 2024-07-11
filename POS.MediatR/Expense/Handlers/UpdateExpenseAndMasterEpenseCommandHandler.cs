using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;
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

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseAndMasterExpenseCommand request, CancellationToken cancellationToken)
        {
            if (request.ExpenseId.HasValue)
            {
                var entityExist = await _expenseRepository.FindAsync(request.ExpenseId.Value);
                if (entityExist == null)
                {
                    _logger.LogError("Expense does not exists.");
                    return ServiceResponse<bool>.Return409("Expense does not exists.");
                }
                entityExist.AccountStatus = request.AccountStatus;
                entityExist.AccountStatusRemarks = request.AccountStatusRemarks;
                entityExist.ReimbursementAmount = request.ReimbursementAmount;

                var masterEntityExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
                masterEntityExist.ReimbursementAmount = request.ReimbursementAmount + masterEntityExist.ReimbursementAmount;
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
                //_mapper.Map(request, entityExist);            

                _expenseRepository.Update(entityExist);
                _masterExpenseRepository.Update(masterEntityExist);
            }
            if (request.MasterExpenseId.HasValue)
            {
                var masterEntityExist = await _masterExpenseRepository.FindAsync(request.MasterExpenseId.Value);
                masterEntityExist.ReimbursementAmount = request.ReimbursementAmount;
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
                if(!string.IsNullOrEmpty(request.ReimbursementRemarks))
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
                    requestWallet.CurrentWalletBalance = (amount + request.ReimbursementAmount);
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

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
