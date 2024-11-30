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
        public string? Status { get; set; }
        public string? ApprovalStage { get; set; }
        public string? ExpenseType { get; set; }
        public string? ExpenseByUser { get; set; }
        public string? ExpenseStatus { get; set; }
        public bool? IsMyRequest { get; set; }
        public string? BranchName { get; set; }
        public Guid? BranchId { get; set; }
        public string? JourneyNumber { get; set; }
        public int? AccountApprovalStage { get; set; }
        public string? AccountApprovalStatus { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string? AccountTeam { get; set; }
    }
}
