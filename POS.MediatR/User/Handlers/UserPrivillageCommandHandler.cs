using BTTEM.MediatR.User.Commands;
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

namespace BTTEM.MediatR.User.Handlers
{
    public class UserPrivillageCommandHandler : IRequestHandler<UserPrivillageCommand, ServiceResponse<bool>>
    {
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UserPrivillageCommandHandler> _logger;
        public UserPrivillageCommandHandler(IUnitOfWork<POSDbContext> uow, ILogger<UserPrivillageCommandHandler> logger, IRoleClaimRepository roleClaimRepository)
        {
            _uow = uow;
            _logger = logger;
            _roleClaimRepository = roleClaimRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UserPrivillageCommand request, CancellationToken cancellationToken)
        {
            var result = _roleClaimRepository.All.Any(x => x.RoleId == request.RoleId && x.ClaimType == request.ActionKey);
            return ServiceResponse<bool>.ReturnResultWith200(result);
        }
    }
}
