using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class DashboardData
    {
        public string UserName { get; set; }
        public int TotalTrip { get; set; } = 0;
        public decimal TotalAdvanceMoney { get; set; } = 0;
        public decimal TotalReimbursementAmount { get; set; } = 0;
        public decimal TotalExpenseAmount { get; set; }
        public int TotalApprovedExpense { get; set; } = 0;
        public int TotalPartialApprovedExpense { get; set; } = 0;
        public int TotalPendingExpense { get; set; } = 0;
    }
}
