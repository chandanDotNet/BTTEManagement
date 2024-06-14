using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Data;
using BTTEM.MediatR.Commands;
using POS.Repository;
using Microsoft.EntityFrameworkCore;

namespace BTTEM.MediatR.Handlers
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ServiceResponse<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<POS.Data.User> _userManager;
        private readonly ILogger<ForgetPasswordCommandHandler> _logger;
        public ForgetPasswordCommandHandler(
            UserManager<POS.Data.User> userManager,
            ILogger<ForgetPasswordCommandHandler> logger,
            IUserRepository userRepository
            )
        {
            _userManager = userManager;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            //var entity = await _userManager.FindByEmailAsync(request.UserName);
            var entity = await _userRepository.All.Where(u => u.UserName == request.UserName && u.OTP == request.OTP).FirstOrDefaultAsync();
            if (entity == null)
            {
                _logger.LogError("Invalid OTP.");
                return ServiceResponse<bool>.ReturnFailed(404, "Invalid OTP.");
            }
            string code = await _userManager.GeneratePasswordResetTokenAsync(entity);
            IdentityResult passwordResult = await _userManager.ResetPasswordAsync(entity, code, request.Password);
            if (!passwordResult.Succeeded)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}