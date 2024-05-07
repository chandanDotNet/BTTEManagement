using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
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

namespace BTTEM.MediatR.Trip.Handlers
{
    public class AddTripCommandHandler : IRequestHandler<AddTripCommand, ServiceResponse<TripDto>>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTripCommandHandler> _logger;

        public AddTripCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddTripCommandHandler> logger
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<TripDto>> Handle(AddTripCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<BTTEM.Data.Trip>(request);

            if (!string.IsNullOrWhiteSpace(request.Name))
            {

                var id = Guid.NewGuid();
                entity.Id = id;
                entity.Status = "Confirmed";
                entity.Approval = "Department Head";

            }

            _tripRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip");
                return ServiceResponse<TripDto>.Return500();
            }

            var trip = _mapper.Map<TripDto>(entity);
            return ServiceResponse<TripDto>.ReturnResultWith200(trip);
        }



    }
}
