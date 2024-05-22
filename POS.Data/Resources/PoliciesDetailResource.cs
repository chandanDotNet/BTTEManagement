using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class PoliciesDetailResource : ResourceParameters
    {
        public PoliciesDetailResource() : base("Name")
        {

        }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? GradeId { get; set; }
        public Guid? CompanyAccountId { get; set; }

    }
}
