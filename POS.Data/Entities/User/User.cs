using Microsoft.AspNetCore.Identity;
using POS.Data.Dto;
using System;
using System.Collections.Generic;

namespace POS.Data
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePhoto { get; set; }
        public string Provider { get; set; }
        public string Address { get; set; }
        public bool IsSuperAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
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
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
       // public List<Role> Roles { get; set; } = new List<Role>();
        //public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
