using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class BusinessAreaResource : ResourceParameters
    {
        public BusinessAreaResource() : base("BusinessAreaName")
        {

        }
        public Guid? Id { get; set; }
        public string SearchQuery { get; set; }
        public Guid? CompanyAccountId { get; set; }
    }
}
