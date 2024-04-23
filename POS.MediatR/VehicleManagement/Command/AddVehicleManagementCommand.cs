using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddVehicleManagementCommand : IRequest<ServiceResponse<VehicleManagementDto>>
    {
        public string Name { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
