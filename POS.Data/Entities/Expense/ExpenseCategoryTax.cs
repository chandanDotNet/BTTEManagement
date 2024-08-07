using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ExpenseCategoryTax : BaseEntity
    {
        public Guid ExpenseCategoryId { get; set; }
        public Guid TaxId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public Tax Tax { get; set; }
    }
}
