using BTTEM.MediatR.Branch.Command;
using BTTEM.MediatR.CostCenter.Command;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CostCenter.Handler
{
    public class GetAllCostCenterQueryCommandHandler : IRequestHandler<GetAllCostCenterQueryCommand, CostCenterList>
    {
        private readonly ICostCenterRepository _costCenterRepository;
        public GetAllCostCenterQueryCommandHandler(ICostCenterRepository costCenterRepository)
        {
            _costCenterRepository = costCenterRepository;
        }

        public async Task<CostCenterList> Handle(GetAllCostCenterQueryCommand request, CancellationToken cancellationToken)
        {
            return await _costCenterRepository.GetCostCenters(request.CostCenterResource);
        }
    }
}
