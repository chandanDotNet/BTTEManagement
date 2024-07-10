using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class LocalConveyanceExpense : BaseEntity
    {

        public Guid Id { get; set; }
        public Guid MasterExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Particular { get; set; }
        public string Place { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal ApproxKM { get; set; } = 0;
        public string ModeOfTransport { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal GrandTotal { get; set; } = 0;
        public string Status { get; set; }
        public string Remarks { get; set; }
        public List<LocalConveyanceExpenseDocument> Documents { get; set; }

    }
}
