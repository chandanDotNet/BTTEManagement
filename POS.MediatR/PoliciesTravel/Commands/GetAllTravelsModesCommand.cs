using BTTEM.Data.Dto;
using BTTEM.Data.Dto.PoliciesTravel;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class GetAllTravelsModesCommand : IRequest<List<TravelModeDto>>
    {
        public Guid? Id { get; set; }

    }
}
