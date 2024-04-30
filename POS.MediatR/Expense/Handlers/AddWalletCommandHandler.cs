using AutoMapper;
using BTTEM.Repository;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand, ServiceResponse<WalletDto>>
    {

        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddWalletCommandHandler> _logger;
        private readonly IUserRepository _userRepository;
        public AddWalletCommandHandler(
           IWalletRepository walletRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddWalletCommandHandler> logger,
             IUserRepository userRepository
            )
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<WalletDto>> Handle(AddWalletCommand request, CancellationToken cancellationToken)
        {
            decimal amount = 0;
            var appUser = await _userRepository.FindAsync(request.UserId);
            if (appUser != null && appUser.IsPermanentAdvance==true)
            {
                amount = appUser.PermanentAdvance.Value;
            }
            if(request.IsCredit)
            {
                //request.PermanentAdvance = amount;
                request.CurrentWalletBalance = (amount + request.ExpenseAmount);
            }
            else
            {
               // request.PermanentAdvance = amount;
                request.CurrentWalletBalance = (amount - request.ExpenseAmount);
            }
            var entity = _mapper.Map<Wallet>(request);
            _walletRepository.Add(entity);

            appUser.PermanentAdvance = request.CurrentWalletBalance;
            _userRepository.Update(appUser);
            // remove other as default

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<WalletDto>.Return500();
            }
            var entityDto = _mapper.Map<WalletDto>(entity);
            return ServiceResponse<WalletDto>.ReturnResultWith200(entityDto);
        }

    }
}
