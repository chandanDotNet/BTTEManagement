using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class CarBikeLogBookExpenseResource : ResourceParameters
    {

        public CarBikeLogBookExpenseResource() : base("CreatedDate")
        {
        }
        public Guid? Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ReportingHeadId { get; set; }
        public bool? IsReport { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsMyRequest { get; set; }
    }
}
