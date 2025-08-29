using BTTEM.Data.Dto;
using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class MasterExpenseDto
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
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public UserDto CreatedByUser { get; set; }
        public Guid ExpenseId { get; set; }
        public string ReimbursementStatus { get; set; }
        public List<ExpenseDto> Expenses { get; set; }
        public bool IsExpenseCompleted { get; set; }
        public int NoOfPendingAction { get; set; } = 0;
        public int NoOfPendingReimbursementAction { get; set; } = 0;
        public Trip Trip { get; set; }
        public string JourneyNumber { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? ReimbursementRemarks { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool IsGroupExpense { get; set; }
        public string NoOfPerson { get; set; }
        public Guid? AccountsCheckerOneId { get; set; }
        public string LevelOneUser { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountsCheckerOneStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public Guid? AccountsCheckerTwoId { get; set; }
        public string LevelTwoUser { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountsCheckerTwoStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public Guid? AccountsCheckerThreeId { get; set; }
        public string LevelThreeUser { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountsCheckerThreeStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool? IsExpenseChecker { get; set; }
        public int? AccountsApprovalStage { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public decimal FirstLevelReimbursementAmount { get; set; } = 0;
        public decimal SecondLevelReimbursementAmount { get; set; } = 0;
        public decimal ThirdLevelReimbursementAmount { get; set; } = 0;
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        public bool IsExpenseChecked { get; set; } = false;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountTeam { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List<GroupExpenseDto> GroupExpenses { get; set; }
        public bool? IsGroupTrip { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List<GroupTripDto>? GroupTrips { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public CompanyAccountDto CompanyAccount { get; set; }
        public CompanyAccountDto BillingCompanyAccount { get; set; }
        public decimal TotalDeviation { get; set; } = 0;
        public string Remarks { get; set; }
    }
}
