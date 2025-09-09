using BTTEM.MediatR.BusinessArea.Command;
using BTTEM.MediatR.CostCenter.Command;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.BusinessArea.Handler
{
    public class GetAllBusinessAreaQueryCommandHandler : IRequestHandler<GetAllBusinessAreaQueryCommand, BusinessAreaList>
    {
        private readonly IBusinessAreaRepository _businessAreaRepository;
        public GetAllBusinessAreaQueryCommandHandler(IBusinessAreaRepository businessAreaRepository)
        {
            _businessAreaRepository = businessAreaRepository;
        }

        public async Task<BusinessAreaList> Handle(GetAllBusinessAreaQueryCommand request, CancellationToken cancellationToken)
        {
            return await _businessAreaRepository.GetBusinessAreas(request.BusinessAreaResource);
        }
    }
}
