using MediatR;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.MMT.Handler
{
    public class GetAllMMTCityQueryHandler : IRequestHandler<GetAllMMTCityQuery, MMTCityList>
    {
        private readonly IMMTCityRepository _mmtCityRepository;
        public GetAllMMTCityQueryHandler(IMMTCityRepository mmtCityRepository)
        {
            _mmtCityRepository = mmtCityRepository;
        }
        public async Task<MMTCityList> Handle(GetAllMMTCityQuery request, CancellationToken cancellationToken)
        {
            return await _mmtCityRepository.GetMMTCities(request.MMTCityResource);
        }
    }
}
