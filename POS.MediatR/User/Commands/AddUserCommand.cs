using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;
using POS.Helper;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class AddUserCommand : IRequest<ServiceResponse<UserDto>>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? AlternateEmail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public string? Address { get; set; }
        public bool? IsImageUpdate { get; set; }
        public string? ImgSrc { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmployeeCode { get; set; }
        public string? PanNo { get; set; }
        public string? AadhaarNo { get; set; }
        public Guid? Department { get; set; }
        public Guid? GradeId { get; set; }
        public Guid? EmpGradeId { get; set; }
        public string? Designation { get; set; }
        public string? BankName { get; set; }
        public string? IFSC { get; set; }
        public string? BranchName { get; set; }
        public string? AccountType { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public string? SapCode { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public Guid? ReportingTo { get; set; }
        public string? ReportingToName { get; set; }
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
        public string? AccountTeam { get; set; }
        public bool IsDirector { get; set; }
        public int CalenderDays { get; set; }
        public List<UserRoleDto>? UserRoles { get; set; } = new List<UserRoleDto>();

    }
}
