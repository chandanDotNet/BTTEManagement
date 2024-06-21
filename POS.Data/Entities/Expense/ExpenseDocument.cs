using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Entities.Expense
{
    public class ExpenseDocument
    {

        public Guid Id { get; set; }
        public Guid? ExpenseId { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
    }
}
