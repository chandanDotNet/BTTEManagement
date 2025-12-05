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

        public Guid? ApprovedByFirstLevel { get; set; }
        public decimal? ReimbursementAmountFirstLevel { get; set; } = 0;
        public string? AccountStatusFirstLevel { get; set; }
        public string? AccountStatusRemarksFirstLevel { get; set; }
        public string? ReimbursementStatusFirstLevel { get; set; }
        public string? ReimbursementRemarksFirstLevel { get; set; }

        public Guid? ApprovedBySecondLevel { get; set; }
        public decimal? ReimbursementAmountSecondLevel { get; set; } = 0;
        public string? AccountStatusSecondLevel { get; set; }
        public string? AccountStatusRemarksSecondLevel { get; set; }
        public string? ReimbursementStatusSecondLevel { get; set; }
        public string? ReimbursementRemarksSecondLevel { get; set; }

        public Guid? ApprovedByThirdLevel { get; set; }
        public decimal? ReimbursementAmountThirdLevel { get; set; } = 0;
        public string? AccountStatusThirdLevel { get; set; }
        public string? AccountStatusRemarksThirdLevel { get; set; }
        public string? ReimbursementStatusThirdLevel { get; set; }
        public string? ReimbursementRemarksThirdLevel { get; set; }

        public int? ExpenseApprovalStage { get; set; }= 0;

        public List<ExpenseDocument> ExpenseDocument { get; set; }
        public List<ExpenseDetail> ExpenseDetail { get; set; }
        public decimal DeviationAmount { get; set; }
        public string? Allowance { get; set; }
        public Guid? BusinessAreaId { get; set; }
        public string? CostCenter { get; set; }
    }
}
