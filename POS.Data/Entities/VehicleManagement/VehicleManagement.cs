﻿using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class VehicleManagement : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
