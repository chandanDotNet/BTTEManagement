using AutoMapper;
using BTTEM.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class DeleteUserCommandHandlerByEmail : IRequestHandler<DeleteUserCommandByEmail, ServiceResponse<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteUserCommandHandlerByEmail> _logger;
        private readonly UserManager<POS.Data.User> _userManager;
        private readonly UserInfoToken _userInfoToken;
        public DeleteUserCommandHandlerByEmail(IMapper mapper,
            ILogger<DeleteUserCommandHandlerByEmail> logger,
            UserManager<POS.Data.User> userManager, UserInfoToken userInfoToken)
        {
            _mapper = mapper;
            _logger = logger;
            _userInfoToken = userInfoToken;
            _userManager = userManager;
        }
        public async Task<ServiceResponse<UserDto>> Handle(DeleteUserCommandByEmail request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.Users.Where(x => x.UserName == request.Email.ToString()).FirstOrDefaultAsync();
            if (appUser == null)
            {
                _logger.LogError("User does not exist.");
                return ServiceResponse<UserDto>.Return409("User does not exist.");
            }
            appUser.IsDeleted = true;
            appUser.DeletedDate = DateTime.Now;
            appUser.DeletedBy = Guid.Parse(_userInfoToken.Id);
            IdentityResult result = await _userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                return ServiceResponse<UserDto>.Return500();
            }

            return ServiceResponse<UserDto>.ReturnResultWith200(_mapper.Map<UserDto>(appUser));
        }
    }
}
