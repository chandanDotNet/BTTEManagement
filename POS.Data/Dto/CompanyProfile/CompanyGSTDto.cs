using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class CompanyGSTDto
    {
        public Guid? Id { get; set; }
        public Guid? StateId { get; set; }
        public string StateName { get; set; }
        public string GSTNo { get; set; }
        public Guid? CompanyAccountId { get; set; }
    }
}
