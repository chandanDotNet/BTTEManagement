using BTTEM.Data;
using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddMasterExpenseCommand : IRequest<ServiceResponse<MasterExpenseDto>>
    {

        public Guid? Id { get; set; }
        public string ExpenseNo { get; set; }
        public Guid? TripId { get; set; }
        public string Name { get; set; }
        public string ExpenseType { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReimbursementAmount { get; set; }
        public decimal AdvanceMoney { get; set; }
        public string ApprovalStage { get; set; }
        public int NoOfBill { get; set; }
        public string ExpenseByUser { get; set; }
        public bool IsGroupExpense { get; set; }
        public string NoOfPerson { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public string? AccountsCheckerOneStatus { get; set; }
        public string? AccountsCheckerTwoStatus { get; set; }
        public string? AccountsCheckerThreeStatus { get; set; }
        public bool? IsExpenseChecker { get; set; } = false;
        public int? AccountsApprovalStage { get; set; } = 0;
        public string ReceiptName { get; set; }
        public string DocumentData { get; set; }
        public bool IsExpenseChecked { get; set; } = false;
        public string? AccountTeam { get; set; }
        public List<GroupExpenseDto> GroupExpenses { get; set; }
        public List<AddExpenseCommand> ExpenseDetails { get; set; }
    }
}
