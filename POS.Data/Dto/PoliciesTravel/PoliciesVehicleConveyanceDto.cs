﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class PoliciesVehicleConveyanceDto
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public Guid PoliciesDetailId { get; set; }
        public decimal? RatePerKM { get; set; }
        public decimal? MaintenanceCharges { get; set; }
        public decimal? CompanyVehicleRatePerKM { get; set; }
        public bool IsCompanyVehicle { get; set; } = false;
        public bool IsDeleted { get; set; }

    }
}
