﻿using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace POS.MediatR.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResponse<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateUserCommandHandler(
            IUserRoleRepository userRoleRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserManager<User> userManager,
            UserInfoToken userInfoToken,
            ILogger<UpdateUserCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            //string oldProfilePhoto = string.Empty;
            var appUser = await _userManager.FindByIdAsync(request.Id.ToString());
            if (appUser == null)
            {
                _logger.LogError("User does not exist.");
                return ServiceResponse<UserDto>.Return409("User does not exist.");
            }
            //if (request.FirstName == "Sync")
            //{
            //    //appUser.ReportingTo = request.ReportingTo;
            //}
            //else
            //{
            appUser.FirstName = request.FirstName;
            appUser.LastName = request.LastName;
            appUser.PhoneNumber = request.PhoneNumber;
            appUser.Address = request.Address;
            appUser.IsActive = request.IsActive;
            //appUser.ModifiedDate = DateTime.UtcNow;
            appUser.ModifiedDate = DateTime.Now;
            appUser.ModifiedBy = Guid.Parse(_userInfoToken.Id);
            appUser.DateOfJoining = request.DateOfJoining;
            appUser.DateOfBirth = request.DateOfBirth;
            appUser.EmployeeCode = request.EmployeeCode;
            appUser.AadhaarNo = request.AadhaarNo;
            appUser.PanNo = request.PanNo;
            appUser.Department = request.Department;
            appUser.GradeId = request.GradeId;
            appUser.EmpGradeId = request.EmpGradeId;
            appUser.Designation = request.Designation;

            appUser.BankName = request.BankName;
            appUser.IFSC = request.IFSC;
            appUser.BranchName = request.BranchName;
            appUser.AccountType = request.AccountType;
            appUser.AccountName = request.AccountName;
            appUser.AccountNumber = request.AccountNumber;
            appUser.SapCode = request.SapCode;
            appUser.CompanyAccountId = request.CompanyAccountId;
            appUser.CompanyAccountBranchId = request.CompanyAccountBranchId;
            appUser.IsPermanentAdvance = request.IsPermanentAdvance;
            appUser.PermanentAdvance = request.PermanentAdvance;
            appUser.ReportingTo = request.ReportingTo;
            appUser.ReportingToName = request.ReportingToName;
            appUser.VendorCode = request.VendorCode;
            appUser.FrequentFlyerNumber = request.FrequentFlyerNumber;
            appUser.ApprovalLevel = request.ApprovalLevel;
            appUser.IsCompanyVehicleUser = request.IsCompanyVehicleUser;
            appUser.AlternateEmail = request.AlternateEmail;
            appUser.AccountTeam = request.AccountTeam;            
            appUser.IsDirector = request.IsDirector;
            appUser.CalenderDays = request.CalenderDays;
            string oldProfilePhoto = appUser.ProfilePhoto;
            //oldProfilePhoto = appUser.ProfilePhoto;
            if (request.IsImageUpdate)
            {
                if (!string.IsNullOrEmpty(request.ImgSrc))
                {
                    appUser.ProfilePhoto = $"{Guid.NewGuid()}.png";
                }
                else
                {
                    appUser.ProfilePhoto = null;
                }
            }
            //}

            IdentityResult result = await _userManager.UpdateAsync(appUser);

            // Update User's Role
            var userRoles = _userRoleRepository.All.Where(c => c.UserId == appUser.Id).ToList();
            var rolesToAdd = request.UserRoles.Where(c => !userRoles.Select(c => c.RoleId).Contains(c.RoleId)).ToList();
            _userRoleRepository.AddRange(_mapper.Map<List<UserRole>>(rolesToAdd));
            var rolesToDelete = userRoles.Where(c => !request.UserRoles.Select(cs => cs.RoleId).Contains(c.RoleId)).ToList();
            _userRoleRepository.RemoveRange(rolesToDelete);

            if (await _uow.SaveAsync() <= 0 && !result.Succeeded)
            {
                return ServiceResponse<UserDto>.Return500();
            }

            if (request.IsImageUpdate)
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                // delete old file
                if (!string.IsNullOrWhiteSpace(oldProfilePhoto))
                {
                    var oldFile = Path.Combine(contentRootPath, _pathHelper.UserProfilePath, oldProfilePhoto);
                    if (File.Exists(oldFile))
                    {
                        FileData.DeleteFile(oldFile);
                    }
                }
                // save new file
                if (!string.IsNullOrWhiteSpace(request.ImgSrc))
                {
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.UserProfilePath, appUser.ProfilePhoto);
                    await FileData.SaveFile(pathToSave, request.ImgSrc);
                }
            }
            return ServiceResponse<UserDto>.ReturnResultWith200(_mapper.Map<UserDto>(appUser));
        }
    }
}
