using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class BranchResource : ResourceParameters
    {
        public BranchResource() : base("Name")
        {

        }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
