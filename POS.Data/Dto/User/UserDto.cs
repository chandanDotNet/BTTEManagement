using BTTEM.Data;
using BTTEM.Data.Dto;
using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Provider { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmployeeCode { get; set; }
        public string? PanNo { get; set; }
        public string? AadhaarNo { get; set; }
        public Guid? Department { get; set; }
        public Department Departments { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? GradeId { get; set; }
        public GradeDto Grades { get; set; }
        public string? GradeName { get; set; }
        public string? Designation { get; set; }
        public string? BankName { get; set; }
        public string? IFSC { get; set; }
        public string? BranchName { get; set; }
        public string? AccountType { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public string? SapCode { get; set; }
        public Guid? CompanyAccountId { get; set; }
       
        public CompanyAccountDto CompanyAccounts { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public Branch CompanyAccountBranch { get; set; }
        public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
        public List<UserRoleDto> UserRoless { get; set; } = new List<UserRoleDto>();
        public List<UserClaimDto> UserClaims { get; set; } = new List<UserClaimDto>();
        public List<RoleDto> Role { get; set; } = new List<RoleDto>();
        public List<string> RoleName { get; set; }
        public string? UserRoleName { get; set; }
        public Guid? ReportingTo { get; set; }
        public string? ReportingToName { get; set; }
        public Guid? EmpGradeId { get; set; }
        public EmpGrade EmpGrades { get; set; }
        public string? EmpGradeName { get; set; }
        public bool? IsPermanentAdvance { get; set; }
        public decimal? PermanentAdvance { get; set; }
        public string? VendorCode { get; set; }
        public int? HrmsReportingHeadCode { get; set; }
        public int? HrmsId { get; set; }
        public int? HrmsUser { get; set; }
        public int? HrmsDepartmentId { get; set; }
        public int? HrmsGradeId { get; set; }
        public string? FrequentFlyerNumber { get; set; }
        public int? ApprovalLevel { get; set; }
        public bool IsCompanyVehicleUser { get; set; }
        // public UserDto ReportingToDetails { get; set; }

    }
}
