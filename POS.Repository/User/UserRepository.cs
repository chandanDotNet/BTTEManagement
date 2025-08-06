using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using POS.Data.Resources;
using BTTEM.Repository;
using AutoMapper;
using BTTEM.Data.Resources;

namespace POS.Repository
{
    public class UserRepository : GenericRepository<User, POSDbContext>,
          IUserRepository
    {
        private JwtSettings _settings = null;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        //private readonly IGradeRepository _gradeRepository;
        public UserRepository(
            IUnitOfWork<POSDbContext> uow,
             JwtSettings settings,
             IUserClaimRepository userClaimRepository,
             IRoleClaimRepository roleClaimRepository,
             IUserRoleRepository userRoleRepository,
             IActionRepository actionRepository,
             IPropertyMappingService propertyMappingService,
             IDepartmentRepository departmentRepository,
             IMapper mapper
            //IGradeRepository gradeRepository
            ) : base(uow)
        {
            _roleClaimRepository = roleClaimRepository;
            _userClaimRepository = userClaimRepository;
            _userRoleRepository = userRoleRepository;
            _settings = settings;
            _actionRepository = actionRepository;
            _propertyMappingService = propertyMappingService;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            //_gradeRepository = gradeRepository;
        }

        public async Task<UserList> GetUsers(UserResource userResource)
        {
            //var collectionBeforePaging = All.Include(c => c.Grades).Include(c=>c.UserRoles).ThenInclude(a=>a.Role);

            //collectionBeforePaging.Where(u => u.Email == userResource.Name); 
            //var collectionBeforePaging = All.ApplySort(userResource.OrderBy, _propertyMappingService.GetPropertyMapping<UserDto, User>());
            //var collectionBeforePaging = AllIncluding(g=>g.Grades,eg=>eg.EmpGrades,d=>d.Departments).ApplySort(userResource.OrderBy, _propertyMappingService.GetPropertyMapping<UserDto, User>());
            var collectionBeforePaging = All.Include(g=>g.Grades).Include(eg => eg.EmpGrades).Include(d => d.Departments).Include(d => d.CompanyAccountBranch).ApplySort(userResource.OrderBy, _propertyMappingService.GetPropertyMapping<UserDto, User>());
            // collectionBeforePaging =collectionBeforePaging.ApplySort(userResource.OrderBy, _propertyMappingService.GetPropertyMapping<UserDto, User>());

            if (!string.IsNullOrWhiteSpace(userResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.UserName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.FirstName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.LastName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.FirstName+" "+c.LastName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.PhoneNumber, $"%{userResource.Name}%"));
            }

            if(userResource.CompanyAccountId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CompanyAccountId == userResource.CompanyAccountId);
            }
            if (userResource.CompanyAccountBranchId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CompanyAccountBranchId == userResource.CompanyAccountBranchId);
            }
            if (userResource.DepartmentId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Department == userResource.DepartmentId);
            }
            if (userResource.GradeId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.GradeId == userResource.GradeId);
            }
            if (userResource.EmpGradeId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.EmpGradeId == userResource.EmpGradeId);
            }
            if (userResource.RoleId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.UserRoles.FirstOrDefault().RoleId == userResource.RoleId);
            }
            if (userResource.ReportingTo.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ReportingTo == userResource.ReportingTo);
            }
            if (userResource.Id.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Id == userResource.Id);
            }
            if (!string.IsNullOrWhiteSpace(userResource.Email))
            {
                collectionBeforePaging = collectionBeforePaging.Where(c => c.Email == userResource.Email);
            }
            if (!string.IsNullOrEmpty(userResource.SearchQuery))
            {
                var searchQueryForWhereClause = userResource.SearchQuery
              .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                    EF.Functions.Like(a.FirstName, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.LastName, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.FirstName+" "+a.LastName, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Grades.GradeName, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Departments.DepartmentName, $"%{searchQueryForWhereClause}%")
                    //|| EF.Functions.Like(a.ReportingToName, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Email, $"{searchQueryForWhereClause}%")
                    || (a.PhoneNumber != null && EF.Functions.Like(a.PhoneNumber, $"{searchQueryForWhereClause}%"))
                    || EF.Functions.Like(a.PhoneNumber, $"{searchQueryForWhereClause}%"
                    )
                    );
            }

            var loginAudits = new UserList(_mapper);
            return await loginAudits.Create(
                collectionBeforePaging,
                userResource.Skip,
                userResource.PageSize
                );
        }

        public async Task<UserAuthDto> BuildUserAuthObject(User appUser)
        {
            UserAuthDto ret = new UserAuthDto();
            List<AppClaimDto> appClaims = new List<AppClaimDto>();
            // Set User Properties
            ret.Id = appUser.Id;
            ret.UserName = appUser.UserName;
            ret.FirstName = appUser.FirstName;
            ret.LastName = appUser.LastName;
            ret.Email = appUser.Email;
            ret.PhoneNumber = appUser.PhoneNumber;
            ret.IsAuthenticated = true;
            ret.ProfilePhoto = appUser.ProfilePhoto;

            ret.DateOfBirth = appUser.DateOfBirth;
            ret.DateOfJoining = appUser.DateOfJoining;
            ret.EmployeeCode = appUser.EmployeeCode;
            ret.AadhaarNo= appUser.AadhaarNo;
            ret.PanNo= appUser.PanNo;
            ret.Department=appUser.Department;
            ret.Grade=appUser.GradeId;
            ret.Designation=appUser.Designation;

            ret.BankName = appUser.Designation;
            ret.AccountName = appUser.Designation;
            ret.AccountNumber = appUser.Designation;
            ret.IFSC = appUser.IFSC;
            ret.AccountType = appUser.AccountType;
            ret.BranchName = appUser.BranchName;
            ret.SapCode = appUser.SapCode;
            ret.CompanyAccountId= appUser.CompanyAccountId;
            ret.CompanyAccountBranchId= appUser.CompanyAccountBranchId;
            ret.FrequentFlyerNumber = appUser.FrequentFlyerNumber;
            ret.ApprovalLevel = appUser.ApprovalLevel;
            ret.IsCompanyVehicleUser = appUser.IsCompanyVehicleUser;
            ret.AlternateEmail = appUser.AlternateEmail;
            ret.AccountTeam = appUser.AccountTeam;

            ret.IsDirector = appUser.IsDirector;
            ret.CalenderDays = appUser.CalenderDays;
            ret.DeviceKey = appUser.DeviceKey;
            ret.IsDeviceTypeAndroid = appUser.IsDeviceTypeAndroid;
            ret.Grades = appUser.Grades;

            // Get all claims for this user
            var appClaimDtos = await this.GetUserAndRoleClaims(appUser);
            ret.Claims = appClaimDtos;
            var claims = appClaimDtos.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            //Get Role
            var role = await GetUserRole(appUser);
            ret.UserRoles = role.ToList();
            // Set JWT bearer token
            ret.BearerToken = BuildJwtToken(ret, claims, appUser.Id);
            return ret;
        }

        private async Task<List<AppClaimDto>> GetUserAndRoleClaims(User appUser)
        {
            var userClaims = await _userClaimRepository.FindBy(c => c.UserId == appUser.Id).Select(c => c.ClaimType).ToListAsync();
            var roleClaims = await GetRoleClaims(appUser);
            var finalClaims = userClaims;
            finalClaims.AddRange(roleClaims);
            finalClaims = finalClaims.Distinct().ToList();
            var lstAppClaimDto = finalClaims.Select(c => new AppClaimDto
            {
                ClaimType = c,
                ClaimValue = "true"
            }).ToList();
            return lstAppClaimDto;
        }

        private async Task<List<string>> GetRoleClaims(User appUser)
        {
            var rolesIds = await _userRoleRepository.All.Where(c => c.UserId == appUser.Id)
                .Select(c => c.RoleId)
                .ToListAsync();
            List<RoleClaim> lstRoleClaim = new List<RoleClaim>();
            var roleClaims = await _roleClaimRepository.All.Where(c => rolesIds.Contains(c.RoleId)).Select(c => c.ClaimType).ToListAsync();
            return roleClaims;
        }

        private async Task<List<RoleDto>> GetUserRole(User appUser)
        {
            var rolesDetails = await _userRoleRepository.AllIncluding(c => c.Role).Where(d => d.UserId == appUser.Id)
                .ToListAsync();
           
            List<RoleDto> roleDto = new List<RoleDto>();
            foreach (var role in rolesDetails)
            {
                RoleDto rd = new RoleDto();
                rd.Id=role.Role.Id;
                rd.Name=role.Role.Name;
                roleDto.Add(rd);
            }
            // var roleClaims = await _roleClaimRepository.All.Where(c => rolesIds.Contains(c.RoleId)).Select(c => c.ClaimType).ToListAsync();
            return roleDto;
        }

        protected string BuildJwtToken(UserAuthDto authUser, IList<Claim> claims, Guid Id)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_settings.Key));
            claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub.ToString(), Id.ToString()));
            claims.Add(new Claim("Email", authUser.Email));
            claims.Add(new Claim("CompanyAccountId", authUser.CompanyAccountId.ToString()));
            claims.Add(new Claim("AccountTeam", authUser.AccountTeam.IsNullOrEmpty()? "AC1": authUser.AccountTeam));
            //string guid = authUser.CompanyAccountId.ToString();
            // Create the JwtSecurityToken object
            var token = new JwtSecurityToken(
              issuer: _settings.Issuer,
              audience: _settings.Audience,
              claims: claims,
               //notBefore: DateTime.UtcNow,
               //expires: DateTime.UtcNow.AddMinutes(
               //    _settings.MinutesToExpiration),
               notBefore: DateTime.Now,
              expires: DateTime.Now.AddMinutes(
                  _settings.MinutesToExpiration),
              signingCredentials: new SigningCredentials(key,
                          SecurityAlgorithms.HmacSha256)
            );
            // Create a string representation of the Jwt token
            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}
