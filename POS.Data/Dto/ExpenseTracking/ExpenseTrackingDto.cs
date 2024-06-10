using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class ExpenseTrackingDto
    {
        public Guid Id { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public Guid? ExpenseId { get; set; }
        public Guid ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionByName { get; set; }
        public string ProfilePhoto { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string ActionType { get; set; }
        public string ExpenseTypeName { get; set; }
        
    }
}
