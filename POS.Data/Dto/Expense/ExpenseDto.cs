
using BTTEM.Data.Dto;
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
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ReUploadRemarks { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? RejectReason { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public decimal ReimbursementAmount { get; set; } = 0;
        public Guid ApprovedByFirstLevel { get; set; }
        public decimal ReimbursementAmountFirstLevel { get; set; } = 0;
        public string AccountStatusFirstLevel { get; set; }
        public string AccountStatusRemarksFirstLevel { get; set; }
        public string ReimbursementStatusFirstLevel { get; set; }
        public string ReimbursementRemarksFirstLevel { get; set; }

        public Guid ApprovedBySecondLevel { get; set; }
        public decimal ReimbursementAmountSecondLevel { get; set; } = 0;
        public string AccountStatusSecondLevel { get; set; }
        public string AccountStatusRemarksSecondLevel { get; set; }
        public string ReimbursementStatusSecondLevel { get; set; }
        public string ReimbursementRemarksSecondLevel { get; set; }

        public Guid ApprovedByThirdLevel { get; set; }
        public decimal ReimbursementAmountThirdLevel { get; set; } = 0;
        public string AccountStatusThirdLevel { get; set; }
        public string AccountStatusRemarksThirdLevel { get; set; }
        public string ReimbursementStatusThirdLevel { get; set; }
        public string ReimbursementRemarksThirdLevel { get; set; }
        public int ExpenseApprovalStage { get; set; }
        public List<ExpenseDocumentDto> ExpenseDocument { get; set; }
        public List<ExpenseDetailDto> ExpenseDetail { get; set; }
        public decimal LodingMetroCity { get; set; }
        public decimal LodingOtherCity { get; set; }
        public decimal MiscDA { get; set; }
        public decimal FoodingAllowance { get; set; }
        public decimal ConveyanceWithinCity { get; set; }
        public decimal ConveyanceCityOuterArea { get; set; }
        public decimal Deviation { get; set; }
    }
}
