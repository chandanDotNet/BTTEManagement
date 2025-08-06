using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CostCenter.Command
{
    public class GetAllCostCenterQueryCommand : IRequest<CostCenterList>
    {
        public CostCenterResource CostCenterResource { get; set; }
    }
}

