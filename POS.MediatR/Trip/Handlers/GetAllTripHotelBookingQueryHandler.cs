using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto;
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
    public class GetAllTripHotelBookingQueryHandler : IRequestHandler<GetAllTripHotelBookingQuery, List<TripHotelBookingDto>>
    {

        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IItineraryHotelBookingQuotationRepository _itineraryHotelBookingQuotationRepository;
        private readonly ICancelTripItineraryHotelUserRepository _cancelTripItineraryHotelUserRepository;
        private readonly IMapper _mapper;
        public GetAllTripHotelBookingQueryHandler(ITripHotelBookingRepository tripHotelBookingRepository, IMapper mapper,
            IItineraryHotelBookingQuotationRepository itineraryHotelBookingQuotationRepository,
            ICancelTripItineraryHotelUserRepository cancelTripItineraryHotelUserRepository
            )
        {
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _mapper = mapper;
            _cancelTripItineraryHotelUserRepository = cancelTripItineraryHotelUserRepository;
            _itineraryHotelBookingQuotationRepository = itineraryHotelBookingQuotationRepository;

        }

        public async Task<List<TripHotelBookingDto>> Handle(GetAllTripHotelBookingQuery request, CancellationToken cancellationToken)
        {
            List<TripHotelBookingDto> result = new List<TripHotelBookingDto>(new List<TripHotelBookingDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                result = await _tripHotelBookingRepository.All.Include(c => c.City).Include(a=>a.Vendor).Where(a => a.IsDeleted == false).ProjectTo<TripHotelBookingDto>(_mapper.ConfigurationProvider).ToListAsync();

            }
            else
            {
                result = await _tripHotelBookingRepository.All.Include(c => c.City).Include(a => a.Vendor).Where(t => t.TripId == request.Id && t.IsDeleted == false).ProjectTo<TripHotelBookingDto>(_mapper.ConfigurationProvider).ToListAsync();

                foreach (var item in result)
                {
                    var quotation = await _itineraryHotelBookingQuotationRepository.All.Where(x => x.TripHotelBookingId == item.Id).ToListAsync();
                    var data = _mapper.Map<List<ItineraryHotelBookingQuotationDto>>(quotation);
                    item.ItineraryHotelQuotationBooking.AddRange(data);
                }

                foreach (var item in result)
                {
                    var cancelUser = await _cancelTripItineraryHotelUserRepository.All.Where(x => x.TripItineraryId == item.Id).ToListAsync();
                    var cancelData = _mapper.Map<List<CancelTripItineraryHotelUserDto>>(cancelUser);
                    item.CancelTripItineraryHotelUserDto.AddRange(cancelData);
                }
            }

            return _mapper.Map<List<TripHotelBookingDto>>(result);
        }

    }
}
