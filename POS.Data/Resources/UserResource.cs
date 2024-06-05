using System;

namespace POS.Data.Resources
{
    public class UserResource : ResourceParameter
    {
        public UserResource() : base("Email")
        {
        }

        public string Name { get; set; }
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public Guid? GradeId { get; set; }
        public Guid? EmpGradeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? ReportingTo { get; set; }
    }
}
