using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class ApprovalLevelUserDto
    {
        public Guid ApprovalLevelId { get; set; }
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
    }
}
