using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ApprovalLevel : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ApprovalLevelTypeId { get; set; }
        public ApprovalLevelType ApprovalLevelType { get; set; }
        public string LevelName { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public int OrderNo { get; set; }
        public List<ApprovalLevelUser> ApprovalLevelUsers { get; set; }
    }
}
