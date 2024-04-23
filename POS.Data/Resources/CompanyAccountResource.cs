using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class CompanyAccountResource : ResourceParameters
    {
        public CompanyAccountResource() : base("AccountName")
        {

        }
        public string AccountName { get; set; }
    }
}
