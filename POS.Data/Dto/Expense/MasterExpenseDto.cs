using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
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
        //public Guid? CreatedBy { get; set; }
        public UserDto CreatedByUser { get; set; }
        public Guid ExpenseId { get; set; }
        public string ReimbursementStatus { get; set; }
        public List<ExpenseDto> Expenses { get; set; }
        public bool IsExpenseCompleted { get; set; }
        public int NoOfPendingAction { get; set; } = 0;
        public Trip Trip { get; set; }
        public string JourneyNumber { get; set; }
    }
}
