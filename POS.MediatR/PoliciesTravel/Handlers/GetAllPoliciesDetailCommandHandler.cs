using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;

using MediatR;
using POS.MediatR.Product.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class GetAllPoliciesDetailCommandHandler : IRequestHandler<GetAllPoliciesDetailCommand, PoliciesDetailList>
    {
        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        public GetAllPoliciesDetailCommandHandler(IPoliciesDetailRepository policiesDetailRepository)
        {
            _policiesDetailRepository = policiesDetailRepository;
        }
        public async Task<PoliciesDetailList> Handle(GetAllPoliciesDetailCommand request, CancellationToken cancellationToken)
        {
            return await _policiesDetailRepository.GetPoliciesDetails(request.PoliciesDetailResource);
        }

    }
}
