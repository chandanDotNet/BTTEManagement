using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class MasterExpense : BaseEntity
    {

        public Guid Id { get; set; }
        public string ExpenseNo { get; set; }
        public Guid? TripId { get; set; }
        public string Name { get; set; }
        public string ExpenseType { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReimbursementAmount { get; set; }
        public decimal? PayableAmount { get; set; }
        public decimal? AdvanceMoney { get; set; }
        public string ApprovalStage { get; set; }
        public Guid? ApprovalStageBy { get; set; }
        public DateTime? ApprovalStageDate { get; set; }
        public int NoOfBill { get; set; }
        public int RollbackCount { get; set; } = 0;
        public string ExpenseByUser { get; set; }
        public string ReimbursementStatus { get; set; }
        public List<Expense> Expenses { get; set; }
        public bool IsExpenseCompleted { get; set; }
        public Trip Trip { get; set; }
        public string? JourneyNumber { get; set; }
        public string? ReimbursementRemarks { get; set; }
        public bool IsGroupExpense { get; set; }
        public string NoOfPerson { get; set; }
        public Guid? AccountsCheckerOneId { get; set; }
        public string? AccountsCheckerOneStatus { get; set; }
        public Guid? AccountsCheckerTwoId { get; set; }
        public string? AccountsCheckerTwoStatus { get; set; }
        public Guid? AccountsCheckerThreeId { get; set; }
        public string? AccountsCheckerThreeStatus { get; set; }
        public bool? IsExpenseChecker { get; set; } = false;
        public int? AccountsApprovalStage { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public decimal FirstLevelReimbursementAmount { get; set; } = 0;
        public decimal SecondLevelReimbursementAmount { get; set; } = 0;
        public decimal ThirdLevelReimbursementAmount { get; set; } = 0;
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        public CompanyAccount CompanyAccounts { get; set; }
        public List<GroupExpense> GroupExpenses { get; set; }
    }
}
