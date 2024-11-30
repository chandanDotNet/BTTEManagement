using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
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
    public class ChangeExpenseAccountTeamCommandHandler : IRequestHandler<ChangeExpenseAccountTeamCommand, ServiceResponse<bool>>
    {

        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly ILogger<ChangeExpenseAccountTeamCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        public ChangeExpenseAccountTeamCommandHandler(IMasterExpenseRepository masterExpenseRepository,
            ILogger<ChangeExpenseAccountTeamCommandHandler> logger,
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

        public async Task<ServiceResponse<bool>> Handle(ChangeExpenseAccountTeamCommand request, CancellationToken cancellationToken)
        {
            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
            var entityExist = await _masterExpenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            if (!string.IsNullOrEmpty(request.AccountTeam))
            {
                entityExist.AccountTeam = request.AccountTeam;
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
