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
        //private readonly IGradeRepository _gradeRepository;
        public UserRepository(
            IUnitOfWork<POSDbContext> uow,
             JwtSettings settings,
             IUserClaimRepository userClaimRepository,
             IRoleClaimRepository roleClaimRepository,
             IUserRoleRepository userRoleRepository,
             IActionRepository actionRepository,
             IPropertyMappingService propertyMappingService,
             IDepartmentRepository departmentRepository
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
            //_gradeRepository = gradeRepository;
        }

        public async Task<UserList> GetUsers(UserResource userResource)
        {
            var collectionBeforePaging = AllIncluding(c=>c.UserRoles);
            collectionBeforePaging =
               collectionBeforePaging.ApplySort(userResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<UserDto, User>());

            if (!string.IsNullOrWhiteSpace(userResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.UserName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.FirstName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.LastName, $"%{userResource.Name}%")
                    || EF.Functions.Like(c.PhoneNumber, $"%{userResource.Name}%"));
            }

            var loginAudits = new UserList(_userRoleRepository, _departmentRepository);
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
            ret.Grade=appUser.Grade;
            ret.Designation=appUser.Designation;
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
            // Create the JwtSecurityToken object
            var token = new JwtSecurityToken(
              issuer: _settings.Issuer,
              audience: _settings.Audience,
              claims: claims,
              notBefore: DateTime.UtcNow,
              expires: DateTime.UtcNow.AddMinutes(
                  _settings.MinutesToExpiration),
              signingCredentials: new SigningCredentials(key,
                          SecurityAlgorithms.HmacSha256)
            );
            // Create a string representation of the Jwt token
            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}
