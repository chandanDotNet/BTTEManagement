using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Handlers;
using BTTEM.Repository;
using MediatR;
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

namespace BTTEM.MediatR.TripTracking.Handler
{
    public class AddTripTrackingCommandHandler : IRequestHandler<AddTripTrackingCommand, ServiceResponse<TripTrackingDto>>
    {
        private readonly ITripTrackingRepository _tripTrackingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTripTrackingCommandHandler> _logger;

        public AddTripTrackingCommandHandler(
           ITripTrackingRepository tripTrackingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddTripTrackingCommandHandler> logger
          )
        {
            _tripTrackingRepository = tripTrackingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<TripTrackingDto>> Handle(AddTripTrackingCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.TripTracking>(request);
            var id = Guid.NewGuid();
            entity.Id = id;
            _tripTrackingRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip");
                return ServiceResponse<TripTrackingDto>.Return500();
            }
            var tripTracking = _mapper.Map<TripTrackingDto>(entity);
            return ServiceResponse<TripTrackingDto>.ReturnResultWith200(tripTracking);
        }



    }
}
