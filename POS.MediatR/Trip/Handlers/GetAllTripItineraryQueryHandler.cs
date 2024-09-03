using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
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
        private readonly IItineraryTicketBookingQuotationRepository _itineraryTicketBookingQuotationRepository;
        private readonly ICancelTripItineraryHotelUserRepository _cancelTripItineraryHotelUserRepository;
        private readonly IMapper _mapper;
        public GetAllTripItineraryQueryHandler(ITripItineraryRepository tripItineraryRepository, IMapper mapper, IItineraryTicketBookingQuotationRepository itineraryTicketBookingQuotationRepository, ICancelTripItineraryHotelUserRepository cancelTripItineraryHotelUserRepository)
        {
            _tripItineraryRepository = tripItineraryRepository;
            _mapper = mapper;
            _itineraryTicketBookingQuotationRepository = itineraryTicketBookingQuotationRepository;
            _cancelTripItineraryHotelUserRepository = cancelTripItineraryHotelUserRepository;
        }

        public async Task<List<TripItineraryDto>> Handle(GetAllTripItineraryQuery request, CancellationToken cancellationToken)
        {
            List<TripItineraryDto> result = new List<TripItineraryDto>(new List<TripItineraryDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                result = await _tripItineraryRepository.All.Include(c => c.ArrivalCity).Include(b => b.DepartureCity)
                               .Include(e => e.ItineraryTicketBooking).ThenInclude(v => v.Vendor).Where(a => a.IsDeleted == false).ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).ToListAsync();

            }
            else
            {
                result = await _tripItineraryRepository.All.Include(c => c.ArrivalCity).Include(b => b.DepartureCity).Include(e => e.ItineraryTicketBooking)
                    .ThenInclude(v => v.Vendor).Where(t => t.TripId == request.Id && t.IsDeleted == false)
                    .ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).ToListAsync();

                foreach (var item in result)
                {
                    var quotation = await _itineraryTicketBookingQuotationRepository.All.Where(x => x.TripItineraryId == item.Id).ToListAsync();
                    var data = _mapper.Map<List<ItineraryTicketBookingQuotationDto>>(quotation);
                    item.ItineraryTicketQuotationBookingDto.AddRange(data);

                    var cancelUser = await _cancelTripItineraryHotelUserRepository.All.Where(x => x.TripItineraryId == item.Id).ToListAsync();
                    var cancelData = _mapper.Map<List<CancelTripItineraryHotelUserDto>>(cancelUser);
                    item.CancelTripItineraryHotelUserDto.AddRange(cancelData);
                }
            }

            return _mapper.Map<List<TripItineraryDto>>(result);
        }
    }
}
