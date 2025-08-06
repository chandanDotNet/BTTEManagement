using MediatR;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Repository;
using BTTEM.MediatR.User.Commands;
using System.Threading;

namespace BTTEM.MediatR.User.Handlers
{
    public class UserMobileCommandHandler :IRequestHandler<UserMobileCommand, ServiceResponse<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UserMobileCommandHandler> _logger;
        public UserMobileCommandHandler(IUserRepository userRepository, IUnitOfWork<POSDbContext> uow,
            ILogger<UserMobileCommandHandler> logger)
        {
            _userRepository = userRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UserMobileCommand request, CancellationToken cancellationToken)
        {
            var isMobileNoExist = false;
            var result = await _userRepository.FindAsync(request.Id);

            if (!string.IsNullOrEmpty(result.PhoneNumber)) {
                isMobileNoExist = true;
            }
            return ServiceResponse<bool>.ReturnResultWith200(isMobileNoExist);
        }
    }
}
