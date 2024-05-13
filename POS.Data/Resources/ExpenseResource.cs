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

    }
}
