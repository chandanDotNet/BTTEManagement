using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class GetAllPoliciesDetailCommand : IRequest<PoliciesDetailList>
    {
        public PoliciesDetailResource PoliciesDetailResource { get; set; }

    }
}
