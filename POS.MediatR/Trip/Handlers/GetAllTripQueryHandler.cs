using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using BTTEM.Repository.Expense;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class GetAllTripQueryHandler : IRequestHandler<GetAllTripQuery, TripList>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        public GetAllTripQueryHandler(ITripRepository tripRepository,IMapper mapper, UserInfoToken userInfoToken)
        {
            _tripRepository = tripRepository;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
        }
        public async Task<TripList> Handle(GetAllTripQuery request, CancellationToken cancellationToken)
        {
            //Guid CreatedBy = Guid.Parse(_userInfoToken.Id);
            //Guid id = cancellationToken.IsCancellationRequested ? Guid.NewGuid() : Guid.Empty;
            return await _tripRepository.GetAllTrips(request.TripResource);
            //List<TripDto> result = new List<TripDto>(new List<TripDto>());
            //if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            //{

            //    if(!request.CreatedBy.HasValue || request.CreatedBy.Value == Guid.Empty)
            //    {
            //        result = await _tripRepository.AllIncluding(c => c.Purpose, v => v.CreatedByUser).Where(t => t.IsDeleted == false).ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();
            //    }
            //    else
            //    {
            //        result = await _tripRepository.AllIncluding(c => c.Purpose, v => v.CreatedByUser).Where(t => t.IsDeleted == false && t.CreatedBy== request.CreatedBy).ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();
            //    }              

            //}
            //else
            //{
            //    result = await _tripRepository.AllIncluding(c => c.Purpose, v => v.CreatedByUser).Where(t=>t.Id== request.Id && t.IsDeleted==false).ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();
            //}

            //return _mapper.Map<List<TripDto>>(result);
        }
    }
}
