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
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.User.Handlers
{
    public class OTPVerificationCommandHandler : IRequestHandler<OTPVerificationCommand, ServiceResponse<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<OTPVerificationCommandHandler> _logger;
        public OTPVerificationCommandHandler(IUserRepository userRepository, IUnitOfWork<POSDbContext> uow,
            ILogger<OTPVerificationCommandHandler> logger)
        {
            _userRepository = userRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(OTPVerificationCommand request, CancellationToken cancellationToken)
        {
            var result = _userRepository.All.Any(u => u.UserName == request.UserName && u.OTP == request.OTP);
            return ServiceResponse<bool>.ReturnResultWith200(result);
        }
    }
}
