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

namespace BTTEM.MediatR.TravelDocument.Handlers
{
    public class GetAllTravelDocumentQueryHandler : IRequestHandler<GetAllTravelDocumentQuery, TravelDocumentList>
    {
        private readonly ITravelDocumentRepository _travelDocumentRepository;
        public GetAllTravelDocumentQueryHandler(ITravelDocumentRepository travelDocumentRepository)
        {
            _travelDocumentRepository = travelDocumentRepository;
        }
        public async Task<TravelDocumentList> Handle(GetAllTravelDocumentQuery request, CancellationToken cancellationToken)
        {
            return await _travelDocumentRepository.GetTravelDocuments(request.TravelDocumentResource);
        }
    }
}
