
using BTTEM.Data.Dto.Expense;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data.Dto
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public Guid? TripId { get; set; }
        public string Name { get; set; }
        public string BillType { get; set; }
        public string GSTNo { get; set; }
        public string Reference { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        public ExpenseCategoryDto ExpenseCategory { get; set; }
        public decimal Amount { get; set; }
        public decimal PayableAmount { get; set; } = 0;
        public Guid? ExpenseById { get; set; }
        public string Description { get; set; }
        public UserDto ExpenseBy { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        public string Status { get; set; }
        public string AccountStatus { get; set; }
        public string AccountStatusRemarks { get; set; }
        public string? ReUploadRemarks { get; set; }
        public string? RejectReason { get; set; }
        public decimal ReimbursementAmount { get; set; } = 0;
        public List<ExpenseDocumentDto> ExpenseDocument { get; set; }
    }
}
