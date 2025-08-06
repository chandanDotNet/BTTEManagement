using POS.Helper;
using System;

namespace POS.Data.Resources
{
    public class ExpenseResource : ResourceParameters
    {
        public ExpenseResource() : base("CreatedDate")
        {
        }
        public Guid? MasterExpenseId { get; set; }
        public string Reference { get; set; }
        public Guid? ExpenseCategoryId { get; set; }
        public Guid? ReportingHeadId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Description { get; set; }
        public Guid? ExpenseById { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? TripId { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Status { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ApprovalStage { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ExpenseType { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ExpenseByUser { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ExpenseStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool? IsMyRequest { get; set; }
#pragma warning disable CS0108 // 'ExpenseResource.BranchName' hides inherited member 'ResourceParameters.BranchName'. Use the new keyword if hiding was intended.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? BranchName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning restore CS0108 // 'ExpenseResource.BranchName' hides inherited member 'ResourceParameters.BranchName'. Use the new keyword if hiding was intended.
#pragma warning disable CS0108 // 'ExpenseResource.BranchId' hides inherited member 'ResourceParameters.BranchId'. Use the new keyword if hiding was intended.
        public Guid? BranchId { get; set; }
#pragma warning restore CS0108 // 'ExpenseResource.BranchId' hides inherited member 'ResourceParameters.BranchId'. Use the new keyword if hiding was intended.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? JourneyNumber { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public int? AccountApprovalStage { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountApprovalStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public Guid? CompanyAccountId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountTeam { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
