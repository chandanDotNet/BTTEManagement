using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
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
    public class UpdateExpenseStatusCommandHandler : IRequestHandler<UpdateExpenseStatusCommand, ServiceResponse<bool>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseStatusCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;

        public UpdateExpenseStatusCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateExpenseStatusCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IMasterExpenseRepository masterExpenseRepository,
            IUserRepository userRepository,
            IWalletRepository walletRepository
            )
        {
            _expenseRepository = expenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _masterExpenseRepository = masterExpenseRepository;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            var entityMasterExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
            if (request.Status== "APPROVED")
            {               
                if (entityMasterExist != null)
                {
                    if(entityMasterExist.PayableAmount==null) { entityMasterExist.PayableAmount = 0; }
                    entityMasterExist.PayableAmount = entityMasterExist.PayableAmount + request.PayableAmount;
                }
               // _masterExpenseRepository.Update(entityMasterExist);

                ////=================
                //AddWalletCommand requestWallet = new AddWalletCommand();
                //decimal amount = 0;
                //var appUser = await _userRepository.FindAsync(entityMasterExist.CreatedBy);
                //if (appUser != null && appUser.IsPermanentAdvance == true)
                //{
                //    amount = appUser.PermanentAdvance.Value;

                //    requestWallet.CurrentWalletBalance = (amount - request.PayableAmount);
                //    requestWallet.UserId = entityMasterExist.CreatedBy;
                //    requestWallet.IsCredit = false;
                //    requestWallet.PermanentAdvance = amount;
                //    requestWallet.ExpenseAmount = request.PayableAmount;
                //    //var entity = _mapper.Map<Wallet>(request);
                //    //_walletRepository.Add(entity);
                //    appUser.PermanentAdvance = requestWallet.CurrentWalletBalance;
                //    _userRepository.Update(appUser);
                //}
                //else
                //{
                    
                //}
                // request.PermanentAdvance = amount;

                //===========
                entityExist.PayableAmount = request.PayableAmount;
            }
            if (request.Status == "PENDING")
            {
                entityExist.PayableAmount = 0;
            }

                if (!string.IsNullOrEmpty(request.RejectReason))
            {
                entityExist.RejectReason = request.RejectReason;
            }
            if (!string.IsNullOrEmpty(request.Status))
            {
                entityExist.Status = request.Status;
            }            

            _expenseRepository.Update(entityExist);

            //======================

            var payAmount = _expenseRepository.All.Where(a => a.MasterExpenseId == entityExist.MasterExpenseId).Sum(a => a.PayableAmount);
            if (payAmount > 0)
            {
                entityMasterExist.PayableAmount = payAmount;
                _masterExpenseRepository.Update(entityMasterExist);

                //var materExp = _masterExpenseRepository.FindBy(a => a.Id == entityExist.MasterExpenseId).FirstOrDefault();
                //if (materExp != null)
                //{
                //    materExp.PayableAmount = payAmount;
                   


                //}
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
