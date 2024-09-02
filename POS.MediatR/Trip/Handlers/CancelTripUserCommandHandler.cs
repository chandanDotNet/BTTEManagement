using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;
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
    public class CancelTripUserCommandHandler : IRequestHandler<CancelTripUserCommand, ServiceResponse<bool>>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IGroupTripRepository _groupTripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelTripUserCommandHandler> _logger;

        public CancelTripUserCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<CancelTripUserCommandHandler> logger,
           IGroupTripRepository groupTripRepository
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _groupTripRepository = groupTripRepository;

        }

        public async Task<ServiceResponse<bool>> Handle(CancelTripUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Type=="APPROVE")
            {
                var groupTripExist = _groupTripRepository.All.Where(v => v.TripId == request.TripId && v.IsCancelRequest==true).ToList();
                if (groupTripExist.Count > 0)
                {
                    _groupTripRepository.RemoveRange(groupTripExist);
                }
                var entityExist = _tripRepository.FindBy(v => v.Id == request.TripId).FirstOrDefault();
                if (entityExist != null)
                {
                    entityExist.IsGroupTripCancelRequest = false;
                    _tripRepository.Update(entityExist);
                }

            }
            if (request.Type == "REQUEST")
            {
                var entityExist =  _tripRepository.FindBy(v => v.Id == request.TripId).FirstOrDefault();
                if (entityExist != null)
                {
                    entityExist.IsGroupTripCancelRequest = true;
                    _tripRepository.Update(entityExist);
                }
                var groupTripExist = _groupTripRepository.All.Where(v => v.TripId == request.TripId).ToList();
                if (groupTripExist.Count > 0)
                {
                    _groupTripRepository.RemoveRange(groupTripExist);
                }

                if (request.GroupTripsUsers != null)
                {
                    request.GroupTripsUsers.ForEach(item =>
                    {
                        item.TripId = request.TripId;
                        item.Id = Guid.NewGuid();
                    });

                    var groupTrip = _mapper.Map<List<GroupTrip>>(request.GroupTripsUsers);
                    _groupTripRepository.AddRange(groupTrip);
                }

            }
            if (request.Type == "REJECT")
            {
                var entityExist = _tripRepository.FindBy(v => v.Id == request.TripId).FirstOrDefault();
                if (entityExist != null)
                {
                    entityExist.IsGroupTripCancelRequest = false;
                    _tripRepository.Update(entityExist);
                }

            }

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);


        }
    }
}
