using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class LocalConveyanceExpenseResource : ResourceParameters
    {

        public LocalConveyanceExpenseResource() : base("CreatedDate")
        {
        }
        public Guid? Id { get; set; }      
        public Guid? CreatedBy { get; set; }       
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
       
    }
}
