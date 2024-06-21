using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class TripResource : ResourceParameters
    {

        public TripResource() : base("CreatedDate")
        {
        }
        public Guid? Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? TripNo { get; set; }
        public string? TripType { get; set; }
        public string? Name { get; set; }
        public Guid? PurposeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? ReportingHeadId { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public string BookTypeBy { get; set; }
        public string? Approval { get; set; }
        public string? Status { get; set; }
        public string IsTripCompleted { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? PurposeFor { get; set; }
        public string? SourceCityName { get; set; }
        public string? DestinationCityName { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? BillingCompanyAccountId { get; set; }
        public bool? IsMyRequest { get; set; }

        // public bool? TripCompleted { get; set; }


    }
}
