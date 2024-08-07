using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Dto.Expense;
using BTTEM.Data.Entities.Expense;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Expense : BaseEntity
    {
        
        public Guid Id { get; set; }
        
        public Guid MasterExpenseId { get; set; }
        //[ForeignKey("MasterExpenseId")]
        //public MasterExpense MasterExpense { get; set; }
        public Guid? TripId { get; set; }
        public string Name { get; set; }
        public string BillType { get; set; }
        public string GSTNo { get; set; }
        public string Reference { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        [ForeignKey("ExpenseCategoryId")]
        public ExpenseCategory ExpenseCategory { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public decimal PayableAmount { get; set; } = 0;
        public Guid? ExpenseById { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        [ForeignKey("ExpenseById")]
        public User ExpenseBy { get; set; }
        public string Status { get; set; }
        public string AccountStatus { get; set; }
        public string AccountStatusRemarks { get; set; }
        public string? ReUploadRemarks { get; set; }
        public string? RejectReason { get; set; }
        public decimal ReimbursementAmount { get; set; } = 0;
        public List<ExpenseDocument> ExpenseDocument { get; set; }
        public List<ExpenseDetail> ExpenseDetail { get; set; }
    }
}
