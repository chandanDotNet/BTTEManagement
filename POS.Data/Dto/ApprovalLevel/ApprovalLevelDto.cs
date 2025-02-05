using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class ApprovalLevelDto
    {
        public Guid Id { get; set; }
        public Guid ApprovalLevelTypeId { get; set; }
        public ApprovalLevelTypeDto ApprovalLevelType { get; set; }
        public string LevelName { get; set; }
        public Guid RoleId { get; set; }
        public RoleDto Role { get; set; }
        public int OrderNo { get; set; }
        public List<ApprovalLevelUserDto> ApprovalLevelUsers { get; set; }
    }
}
