﻿using POS.Helper;
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
    }
}
