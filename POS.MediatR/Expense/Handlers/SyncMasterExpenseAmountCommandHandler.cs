using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
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
    public class SyncMasterExpenseAmountCommandHandler : IRequestHandler<SyncMasterExpenseAmountCommand, ServiceResponse<bool>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<SyncMasterExpenseAmountCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;

        public SyncMasterExpenseAmountCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<SyncMasterExpenseAmountCommandHandler> logger,
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


        public async Task<ServiceResponse<bool>> Handle(SyncMasterExpenseAmountCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            var entityMasterExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
            var payAmount = _expenseRepository.All.Where(a => a.MasterExpenseId == entityExist.MasterExpenseId).Sum(a => a.PayableAmount);
            
                entityMasterExist.PayableAmount = payAmount;
                _masterExpenseRepository.Update(entityMasterExist);

            

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }



            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
