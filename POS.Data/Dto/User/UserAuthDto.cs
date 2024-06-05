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
        public string PhoneNumber { get; set; }
       
        public bool IsAuthenticated { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmployeeCode { get; set; }
        public string? PanNo { get; set; }
        public string? AadhaarNo { get; set; }
        public Guid? Department { get; set; }
        public Guid? Grade { get; set; }
        public string? Designation { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string BranchName { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SapCode { get; set; }
        public Guid CompanyAccountId { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public string BearerToken { get; set; }
        public List<RoleDto> UserRoles { get; set; } = new List<RoleDto>();
        public List<AppClaimDto> Claims { get; set; }
        
    }
}
