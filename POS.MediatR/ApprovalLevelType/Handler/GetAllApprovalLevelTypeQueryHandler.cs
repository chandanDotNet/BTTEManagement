using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Handler
{
    public class GetAllApprovalLevelTypeQueryHandler : IRequestHandler<GetAllApprovalLevelTypeQuery, ApprovalLevelTypeList>
    {
        private readonly IApprovalLevelTypeRepository _approvalLevelTypeRepository;
        public GetAllApprovalLevelTypeQueryHandler(IApprovalLevelTypeRepository approvalLevelTypeRepository)
        {
            _approvalLevelTypeRepository = approvalLevelTypeRepository;
        }
        public async Task<ApprovalLevelTypeList> Handle(GetAllApprovalLevelTypeQuery request, CancellationToken cancellationToken)
        {
            return await _approvalLevelTypeRepository.GetApprovalLevelTypes(request.ApprovalLevelTypeResource);
        }
    }
}
