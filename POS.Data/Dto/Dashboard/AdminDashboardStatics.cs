using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class AdminDashboardStatics
    {
        public decimal TotalExpenseAmount { get; set; } = 0;
        public decimal TotalReimbursementAmount { get; set; } = 0;
        public int TotalTrip { get; set; } = 0;

        public int TotalExpensePending { get; set; } = 0;
        public int TotalExpenseApproved { get; set; } = 0;

    }
}
