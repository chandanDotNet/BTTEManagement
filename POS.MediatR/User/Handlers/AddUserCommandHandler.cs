﻿using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace POS.MediatR.Handlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ServiceResponse<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMapper _mapper;
        private readonly ILogger<AddUserCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddUserCommandHandler(
            IMapper mapper,
            UserManager<User> userManager,
            UserInfoToken userInfoToken,
            ILogger<AddUserCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _userInfoToken = userInfoToken;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }
        public async Task<ServiceResponse<UserDto>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByNameAsync(request.Email);

            if (appUser != null)
            {
                _logger.LogError("Email already exist for another user.");
                return ServiceResponse<UserDto>.Return409("Email already exist for another user.");
            }

            var entity = _mapper.Map<User>(request);

            //if (appUser != null)
            //{
            //    entity.UserName =
            //          entity.Email.Substring(0, entity.Email.LastIndexOf("@"))
            //        + entity.EmployeeCode + entity.Email.Substring(entity.Email.LastIndexOf("@"), entity.Email.Length - request.Email.LastIndexOf("@"));
            //    entity.Email = request.Email;
            //}

            entity.CreatedBy = Guid.Parse(_userInfoToken.Id);
            entity.ModifiedBy = Guid.Parse(_userInfoToken.Id);
            //entity.CreatedDate = DateTime.UtcNow;
            //entity.ModifiedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity.Id = Guid.NewGuid();

            if (entity.UserRoles == null || entity.UserRoles.Count == 0)
            {
                entity.UserRoles.Add(new UserRole()
                {
                    RoleId = new Guid("E1BD3DCE-EECF-468D-B930-1875BD59D1F4"),
                    UserId = entity.Id                    
                });
            }

            if (!string.IsNullOrEmpty(request.ImgSrc))
            {
                var imgageUrl = $"{Guid.NewGuid()}.png";
                entity.ProfilePhoto = imgageUrl;
            }

            IdentityResult result = await _userManager.CreateAsync(entity);
            if (!result.Succeeded)
            {
                return ServiceResponse<UserDto>.Return500();
            }
            if (!string.IsNullOrEmpty(request.Password))
            {
                string code = await _userManager.GeneratePasswordResetTokenAsync(entity);
                IdentityResult passwordResult = await _userManager.ResetPasswordAsync(entity, code, request.Password);
                if (!passwordResult.Succeeded)
                {
                    return ServiceResponse<UserDto>.Return500();
                }
            }
            if (!string.IsNullOrEmpty(request.ImgSrc))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.UserProfilePath, entity.ProfilePhoto);
                await FileData.SaveFile(pathToSave, request.ImgSrc);
            }
            return ServiceResponse<UserDto>.ReturnResultWith200(_mapper.Map<UserDto>(entity));
        }
    }
}
