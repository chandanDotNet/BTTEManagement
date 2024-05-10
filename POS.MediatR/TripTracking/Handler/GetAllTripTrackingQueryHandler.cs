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

namespace BTTEM.MediatR.TripTracking.Handler
{
    public class GetAllTripTrackingQueryHandler : IRequestHandler<GetAllTripTrackingQuery, TripTrackingList>
    {
        private readonly ITripTrackingRepository _tripTrackingRepository;
        public GetAllTripTrackingQueryHandler(ITripTrackingRepository tripTrackingRepository)
        {
            _tripTrackingRepository = tripTrackingRepository;
        }
        public async Task<TripTrackingList> Handle(GetAllTripTrackingQuery request, CancellationToken cancellationToken)
        {
            return await _tripTrackingRepository.GetTripTrackings(request.TripTrackingResource);
        }
    }
}
