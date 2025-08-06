using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class CostCenterResource : ResourceParameters
    {
        public CostCenterResource() : base("CostCenterBranchName")
        {

        }
        public Guid? Id { get; set; }
        public string SearchQuery { get; set; }
    }
}
