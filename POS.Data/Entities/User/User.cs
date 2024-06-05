using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.AspNetCore.Identity;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("Department")]
        public Department Departments { get; set; }

        public Guid? GradeId { get; set; }
        [ForeignKey("GradeId")]
        public Grade Grades { get; set; }
        public string? Designation { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string BranchName { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SapCode { get; set; }
        public Guid CompanyAccountId { get; set; }
        public CompanyAccount CompanyAccounts { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        //public List<UserRole> UserRoless { get; set; } = new List<UserRole>();
        //public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public Guid? ReportingTo { get; set; }
        public string? ReportingToName { get; set; }
        public Guid? EmpGradeId { get; set; }
        [ForeignKey("EmpGradeId")]
        public EmpGrade EmpGrades { get; set; }
        public bool? IsPermanentAdvance { get; set; }
        public decimal? PermanentAdvance { get; set; }

        public List<TravelDocument> TravelDocuments { get; set; }
        //public UserDto ReportingToDetails { get; set; }
    }
}
