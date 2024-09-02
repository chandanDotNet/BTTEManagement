using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class GetAllTripItineraryQueryHandler : IRequestHandler<GetAllTripItineraryQuery, List<TripItineraryDto>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IMapper _mapper;
        public GetAllTripItineraryQueryHandler(ITripItineraryRepository tripItineraryRepository, IMapper mapper)
        {
            _tripItineraryRepository = tripItineraryRepository;
            _mapper = mapper;
        }

        public async Task<List<TripItineraryDto>> Handle(GetAllTripItineraryQuery request, CancellationToken cancellationToken)
        {
            List<TripItineraryDto> result = new List<TripItineraryDto>(new List<TripItineraryDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                result = await _tripItineraryRepository.All.Include(c => c.ArrivalCity).Include(b => b.DepartureCity)
                               .Include(e => e.ItineraryTicketBooking).ThenInclude(v => v.Vendor).Include(q => q.ItineraryTicketBookingQuotation).Where(a => a.IsDeleted == false).ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).ToListAsync();

            }
            else
            {
                result = await _tripItineraryRepository.All.Include(c => c.ArrivalCity).Include(b => b.DepartureCity).Include(e => e.ItineraryTicketBooking)
                    .ThenInclude(v => v.Vendor).Include(q => q.ItineraryTicketBookingQuotation).Where(t => t.TripId == request.Id && t.IsDeleted == false)
                    .ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).ToListAsync();
            }

            return _mapper.Map<List<TripItineraryDto>>(result);
        }
    }
}
