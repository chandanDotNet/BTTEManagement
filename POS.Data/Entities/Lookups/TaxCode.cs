using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class TaxCode
    {
        public string Name { get; set; }
        public string TaxCodeDetails { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
