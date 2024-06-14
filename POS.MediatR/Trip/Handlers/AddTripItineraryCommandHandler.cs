using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class AddTripItineraryCommandHandler : IRequestHandler<AddTripItineraryCommand, ServiceResponse<TripItineraryDto>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTripItineraryCommandHandler> _logger;

        public AddTripItineraryCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddTripItineraryCommandHandler> logger
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<TripItineraryDto>> Handle(AddTripItineraryCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.TripItinerary)
            {
                var entity = _mapper.Map<Data.TripItinerary>(tv);
                entity.Id = Guid.NewGuid();
                entity.ApprovalStatus = "PENDING";

                _tripItineraryRepository.Add(entity);
            }

            //var entity = _mapper.Map<BTTEM.Data.TripItinerary>(request);            

            //    var id = Guid.NewGuid();
            //    entity.Id = id;

            //_tripItineraryRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Itinerary");
                return ServiceResponse<TripItineraryDto>.Return500();
            }
           var entityRes= await _tripItineraryRepository.AllIncluding(c => c.ArrivalCity, b => b.DepartureCity).ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var tripItinerary = _mapper.Map<TripItineraryDto>(entityRes);
            return ServiceResponse<TripItineraryDto>.ReturnResultWith200(tripItinerary);
        }
    }
}
