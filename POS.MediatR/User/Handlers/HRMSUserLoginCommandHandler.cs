﻿using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.Data.Entities;
using POS.Data;
using POS.Common.UnitOfWork;
using POS.Domain;

namespace POS.MediatR.Handlers
{
    public class HRMSUserLoginCommandHandler : IRequestHandler<HRMSUserLoginCommand, ServiceResponse<UserAuthDto>>
    {

        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILoginAuditRepository _loginAuditRepository;
        private readonly IHubContext<UserHub, IHubClient> _hubContext;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<POSDbContext> _uow;

        public HRMSUserLoginCommandHandler(
            IUserRepository userRepository,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILoginAuditRepository loginAuditRepository,
            IHubContext<UserHub, IHubClient> hubContext,
            PathHelper pathHelper,
             IUnitOfWork<POSDbContext> uow
            )
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _loginAuditRepository = loginAuditRepository;
            _hubContext = hubContext;
            _pathHelper = pathHelper;
            _uow = uow;
        }

        public async Task<ServiceResponse<UserAuthDto>> Handle(HRMSUserLoginCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Accesskey))
            {
                var userInfo1 = await _userRepository
                  .All
                  .Where(c => c.OTP == request.Accesskey)
                  .FirstOrDefaultAsync();
                if (userInfo1 != null)
                {
                    request.UserName = userInfo1.UserName;
                    request.Password = userInfo1.PasswordHash;
                }
            }

            var loginAudit = new LoginAuditDto
            {
                UserName = request.UserName,               
                Status = LoginStatus.Error.ToString()                
            };

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                await _loginAuditRepository.LoginAudit(loginAudit);
                return ServiceResponse<UserAuthDto>.ReturnFailed(401, "UserName Or Password is InCorrect.");
            }

            
            

                // var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                var userInfo = await _userRepository
                   .All
                   .Where(c => c.UserName == request.UserName && c.PasswordHash== request.Password)
                   .FirstOrDefaultAsync();

            if (userInfo!=null)
            {
                //var userInfo = await _userRepository
                //    .All
                //    .Where(c => c.UserName == request.UserName)
                //    .FirstOrDefaultAsync();
                if (!userInfo.IsActive)
                {
                    await _loginAuditRepository.LoginAudit(loginAudit);
                    return ServiceResponse<UserAuthDto>.ReturnFailed(401, "UserName Or Password is InCorrect.");
                }

                loginAudit.Status = LoginStatus.Success.ToString();
                await _loginAuditRepository.LoginAudit(loginAudit);
                var authUser = await _userRepository.BuildUserAuthObject(userInfo);
                var onlineUser = new SignlarUser
                {
                    Email = authUser.Email,
                    Id = authUser.Id.ToString()
                };
                await _hubContext.Clients.All.Joined(onlineUser);
                if (!string.IsNullOrWhiteSpace(authUser.ProfilePhoto))
                {
                    authUser.ProfilePhoto = Path.Combine(_pathHelper.UserProfilePath, authUser.ProfilePhoto);
                }

                string Ukey = Guid.NewGuid().ToString().GetHashCode().ToString("X");
                var encUkey = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Ukey));
                userInfo.OTP = Ukey;
                authUser.Accesskey = encUkey;
                _userRepository.Update(userInfo);
                if (await _uow.SaveAsync() <= 0)
                {
                     return ServiceResponse<UserAuthDto>.Return500();
                }

                return ServiceResponse<UserAuthDto>.ReturnResultWith200(authUser);
            }
            else
            {
                await _loginAuditRepository.LoginAudit(loginAudit);
                return ServiceResponse<UserAuthDto>.ReturnFailed(401, "UserName Or Password is InCorrect.");
            }
        }
    }
}
