using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class UpdateExpenseAndMasterExpenseApprovalLevelCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid? ExpenseId { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public decimal ReimbursementAmount { get; set; }
        public string AccountStatus { get; set; }
        public string AccountStatusRemarks { get; set; }
        public string ReimbursementStatus { get; set; }
        public string ReimbursementRemarks { get; set; }
        public decimal LevelReimbursementAmount { get; set; }
        public Guid ApprovedBy { get; set; }

        //public decimal ReimbursementAmountFirstLevel { get; set; }
        //public string AccountStatusFirstLevel { get; set; }
        //public string AccountStatusRemarksFirstLevel { get; set; }
        //public string ReimbursementStatusFirstLevel { get; set; }
        //public string ReimbursementRemarksFirstLevel { get; set; }
        
        //public decimal ReimbursementAmountSecondLevel { get; set; }
        //public string AccountStatusSecondLevel { get; set; }
        //public string AccountStatusRemarksSecondLevel { get; set; }
        //public string ReimbursementStatusSecondLevel { get; set; }
        //public string ReimbursementRemarksSecondLevel { get; set; }
        
        //public decimal ReimbursementAmountThirdLevel { get; set; }
        //public string AccountStatusThirdLevel { get; set; }
        //public string AccountStatusRemarksThirdLevel { get; set; }
        //public string ReimbursementStatusThirdLevel { get; set; }
        //public string ReimbursementRemarksThirdLevel { get; set; }

        public int ExpenseApprovalStage { get; set; }

        public string checkApproval { get; set; }
    }
}
