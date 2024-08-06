﻿using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class Vendor : BaseEntity
    {
        public Guid Id { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string VendorAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string GSTNo { get; set; }
        public string ResponsiblePersonName { get; set; }
    }
}
