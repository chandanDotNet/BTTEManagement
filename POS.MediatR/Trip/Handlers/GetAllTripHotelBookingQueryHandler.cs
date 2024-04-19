using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
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
        private readonly IMapper _mapper;
        public GetAllTripHotelBookingQueryHandler(ITripHotelBookingRepository tripHotelBookingRepository, IMapper mapper)
        {
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _mapper = mapper;
        }

        public async Task<List<TripHotelBookingDto>> Handle(GetAllTripHotelBookingQuery request, CancellationToken cancellationToken)
        {
            List<TripHotelBookingDto> result = new List<TripHotelBookingDto>(new List<TripHotelBookingDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                result = await _tripHotelBookingRepository.AllIncluding(c => c.City).Where(a => a.IsDeleted == false).ProjectTo<TripHotelBookingDto>(_mapper.ConfigurationProvider).ToListAsync();

            }
            else
            {
                result = await _tripHotelBookingRepository.AllIncluding(c => c.City).Where(t => t.TripId == request.Id && t.IsDeleted == false).ProjectTo<TripHotelBookingDto>(_mapper.ConfigurationProvider).ToListAsync();
            }

            return _mapper.Map<List<TripHotelBookingDto>>(result);
        }

    }
}
