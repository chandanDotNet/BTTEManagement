using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class ExpenseTrackingResource : ResourceParameters
    {
        public ExpenseTrackingResource() : base("CreatedDate")
        {
        }
        public Guid? MasterExpenseId { get; set; }
        public Guid? ExpenseId { get; set; }
        public string ActionType { get; set; }
    }
}
