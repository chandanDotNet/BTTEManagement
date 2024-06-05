using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Data;
using POS.Data.Dto;
using AutoMapper;
using BTTEM.Repository;
using BTTEM.Data.Dto;
using BTTEM.Data;

namespace POS.Repository
{
    public class UserList : List<UserDto>
    {
        //public UserList()
        //{
        //}

        //private readonly IUserRoleRepository _userRoleRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        IMapper _mapper;
        //private readonly IGradeRepository _gradeRepository;
        public UserList(
            //IUserRoleRepository userRoleRepository, 
            //IDepartmentRepository departmentRepository, 
            IMapper mapper)
        {
            //_userRoleRepository = userRoleRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
            //_gradeRepository = gradeRepository;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public UserList(List<UserDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<UserList> Create(IQueryable<User> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new UserList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<User> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        //private  string GetUserRole(Guid UserId)
        //{
        //    var rolesDetails =  _userRoleRepository.AllIncluding(c => c.Role).Where(d => d.UserId == UserId)
        //        .ToList();
        //    string RoleName = null;
        //    List<RoleDto> roleDto = new List<RoleDto>();
        //    foreach (var role in rolesDetails)
        //    {
        //        RoleDto rd = new RoleDto();
        //        rd.Id = role.Role.Id;
        //        rd.Name = role.Role.Name;
        //        roleDto.Add(rd);
        //    }
        //    RoleName= roleDto.FirstOrDefault()?.Name;
        //    // var roleClaims = await _roleClaimRepository.All.Where(c => rolesIds.Contains(c.RoleId)).Select(c => c.ClaimType).ToListAsync();
        //    return RoleName;
        //}

        public async Task<List<UserDto>> GetDtos(IQueryable<User> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new UserDto
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    IsActive = c.IsActive,
                    Designation = c.Designation,
                    GradeId = c.GradeId,
                    Department = c.Department,
                    AadhaarNo = c.AadhaarNo,
                    Address = c.Address,
                    DateOfBirth = c.DateOfBirth,
                    DateOfJoining = c.DateOfJoining,
                    EmployeeCode = c.EmployeeCode,
                    PanNo = c.PanNo,
                    ProfilePhoto = c.ProfilePhoto,
                    //Roles = _userRoleRepository.AllIncluding(r => r.Role).Where(r => r.UserId == c.Id).Select(a=>a.Role.Name).ToList(),
                   // Roles= GetUserRole(c.Id).Result,
                    //DepartmentName= _departmentRepository.All.Where(b => b.Id == c.Department).FirstOrDefault().DepartmentName,
                    //GradeName = _gradeRepository.All.Where(d => d.Id == c.Grade).FirstOrDefault().GradeName,
                     //UserRoleName= _userRoleRepository.AllIncluding(c => c.Role).Where(e => e.UserId == c.Id).FirstOrDefault().Role.Name.ToString(),
                    // UserRoleName= GetUserRole(c.Id).ToString(),

                    Grades = _mapper.Map<GradeDto>(c.Grades),
                    //Grades = c.Grades,
                    GradeName= c.Grades.GradeName,
                    //Departments = _mapper.Map<DepartmentDto>(c.Departments),
                    Departments = c.Departments,
                    CompanyAccounts = _mapper.Map<CompanyAccountDto>(c.CompanyAccounts),
                    CompanyAccountId =c.CompanyAccountId,
                    CompanyAccountBranchId = c.CompanyAccountBranchId,
                    UserRoles= _mapper.Map<List<UserRoleDto>>(c.UserRoles),
                    UserRoleName = c.UserRoles.FirstOrDefault().Role.Name,
                    //RoleId = c.UserRoles.FirstOrDefault().Role.Name,
                    BankName = c.BankName,
                    IFSC = c.IFSC,
                    AccountName = c.AccountName,
                    AccountNumber = c.AccountNumber,
                    BranchName = c.BranchName,
                    AccountType = c.AccountType,
                    ReportingTo=c.ReportingTo,
                    ReportingToName=c.ReportingToName,
                    EmpGradeId = c.EmpGradeId,
                    EmpGrades = c.EmpGrades,
                    EmpGradeName = c.EmpGrades.GradeName,
                    IsPermanentAdvance = c.IsPermanentAdvance,
                    PermanentAdvance = c.PermanentAdvance,
                    DepartmentName=c.Departments.DepartmentName,
                    SapCode= c.SapCode
                    
                    

                })
                .ToListAsync();
            return entities;
        }
    }
}
