using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class GroupExpense
    {
        public Guid Id { get; set; }
        public Guid? MasterExpenseId { get; set; }
        [ForeignKey("MasterExpenseId")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
