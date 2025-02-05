using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class ApprovalLevelTypeDto
    {
        public Guid? Id { get; set; }
        public string TypeName { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyAccount Company { get; set; }
    }
}
