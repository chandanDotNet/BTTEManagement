using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public bool IsCredit { get; set; }
        public decimal PermanentAdvance { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal CurrentWalletBalance { get; set; }

    }
}
