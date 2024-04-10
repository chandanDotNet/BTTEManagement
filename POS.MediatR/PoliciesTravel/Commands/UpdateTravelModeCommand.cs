using BTTEM.Data.Dto.PoliciesTravel;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class UpdateTravelModeCommand : IRequest<ServiceResponse<bool>>
    {

        public List<TravelModeDto> TravelModeData { get; set; }
    }
}
