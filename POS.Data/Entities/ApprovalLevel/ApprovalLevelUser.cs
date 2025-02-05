using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ApprovalLevelUser
    {
        public Guid Id { get; set; }
        public Guid ApprovalLevelId { get; set; }
        [ForeignKey("ApprovalLevelId")]
        public ApprovalLevel ApprovalLevel { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
