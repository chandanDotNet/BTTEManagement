using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class GroupExpenseDto
    {
        public Guid Id { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
