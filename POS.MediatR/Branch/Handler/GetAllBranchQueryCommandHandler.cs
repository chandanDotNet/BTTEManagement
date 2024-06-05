using BTTEM.MediatR.Branch.Command;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Branch.Handler
{
    internal class GetAllBranchQueryCommandHandler : IRequestHandler<GetAllBranchQueryCommand, BranchList>
    {
        private readonly IBranchRepository _branchRepository;
        public GetAllBranchQueryCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<BranchList> Handle(GetAllBranchQueryCommand request, CancellationToken cancellationToken)
        {
            return await _branchRepository.GetBranches(request.BranchResource);
        }
    }
}
