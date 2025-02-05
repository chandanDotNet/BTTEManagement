using BTTEM.MediatR.ApprovalLevel.Command;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Handler
{
    public class GetAllApprovalLevelQueryHandler : IRequestHandler<GetAllApprovalLevelQuery, ApprovalLevelList>
    {
        private readonly IApprovalLevelRepository _approvalLevelRepository;
        public GetAllApprovalLevelQueryHandler(IApprovalLevelRepository approvalLevelRepository)
        {
            _approvalLevelRepository = approvalLevelRepository;
        }
        public async Task<ApprovalLevelList> Handle(GetAllApprovalLevelQuery request, CancellationToken cancellationToken)
        {
            return await _approvalLevelRepository.GetApprovalLevels(request.ApprovalLevelResource);
        }
    }
}