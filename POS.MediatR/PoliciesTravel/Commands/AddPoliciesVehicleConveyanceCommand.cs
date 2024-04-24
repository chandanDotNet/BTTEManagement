using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class AddPoliciesVehicleConveyanceCommand : IRequest<ServiceResponse<PoliciesVehicleConveyanceDto>>
    {

        public List<PoliciesVehicleConveyanceDto> VehicleConveyanceData { get; set; }

    }
}
