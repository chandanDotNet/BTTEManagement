using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class ApprovalLevelTypeResource : ResourceParameters
    {
        public ApprovalLevelTypeResource() : base("TypeName")
        {

        }
        public Guid? Id { get; set; }
        public string TypeName { get; set; }
        public string CompanyName { get; set; }
    }
}
