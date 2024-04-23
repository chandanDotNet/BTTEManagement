using BTTEM.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class GetAllPoliciesVehicleConveyanceCommand : IRequest<List<PoliciesVehicleConveyanceDto>>
    {

        public Guid Id { get; set; }

    }
}
