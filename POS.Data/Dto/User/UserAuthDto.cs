using BTTEM.Data;
using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class UserAuthDto
    {
        public UserAuthDto()
        {
            BearerToken = string.Empty;
        }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string PhoneNumber { get; set; }
       
        public bool IsAuthenticated { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? EmployeeCode { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? PanNo { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AadhaarNo { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public Guid? Department { get; set; }
        public Guid? Grade { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Designation { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string BranchName { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SapCode { get; set; }
        public Guid CompanyAccountId { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public string FrequentFlyerNumber { get; set; }
        public int? ApprovalLevel { get; set; }
        public bool IsCompanyVehicleUser { get; set; }
        public string BearerToken { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountTeam { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Accesskey { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool IsDirector { get; set; }
        public int CalenderDays { get; set; }
        public string DeviceKey { get; set; }
        public bool IsDeviceTypeAndroid { get; set; }
        public Grade Grades { get; set; }
        public List<RoleDto> UserRoles { get; set; } = new List<RoleDto>();
        public List<AppClaimDto> Claims { get; set; }
        
    }
}
