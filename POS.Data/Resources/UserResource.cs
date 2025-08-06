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
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Email { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public Guid? CompanyAccountId { get; set; }
        public Guid? CompanyAccountBranchId { get; set; }
        public Guid? GradeId { get; set; }
        public Guid? EmpGradeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? ReportingTo { get; set; }
    }
}
