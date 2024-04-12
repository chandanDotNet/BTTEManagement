using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.CommandAndQuery;
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
    public class GetAllTripQueryHandler : IRequestHandler<GetAllTripQuery, List<TripDto>>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;
        public GetAllTripQueryHandler(ITripRepository tripRepository,IMapper mapper)
        {
            _tripRepository = tripRepository;
            _mapper = mapper;
        }
        public async Task<List<TripDto>> Handle(GetAllTripQuery request, CancellationToken cancellationToken)
        {
            List<TripDto> result = new List<TripDto>(new List<TripDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                 result = await _tripRepository.AllIncluding(c => c.Purpose).ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();

            }
            else
            {
                result = await _tripRepository.AllIncluding(c => c.Purpose).Where(t=>t.Id== request.Id).ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();
            }
               
            return _mapper.Map<List<TripDto>>(result);
        }
    }
}
