﻿using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddVehicleManagementRateCommand : IRequest<ServiceResponse<VehicleManagementRateDto>>
    {
        public List<VehicleManagementRateDto> VehicleManagementRates { get; set; }
    }
}

