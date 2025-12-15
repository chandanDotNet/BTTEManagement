using POS.Data.Resources;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class VendorResource : ResourceParameters
    {
        public VendorResource() : base("VendorName")
        {

        }
        public string VendorName { get; set; }
        public string? VendorCode { get; set; }
    }
}
