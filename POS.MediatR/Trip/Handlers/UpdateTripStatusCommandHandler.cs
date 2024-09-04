using AutoMapper;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using POS.Common.UnitOfWork;
using POS.Data;
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
    public class UpdateTripStatusCommandHandler : IRequestHandler<UpdateTripStatusCommand, ServiceResponse<bool>>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripStatusCommandHandler> _logger;

        public UpdateTripStatusCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripStatusCommandHandler> logger
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripStatusCommand request, CancellationToken cancellationToken)
        {

            var entityExist = await _tripRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(request.Status))
            {
                entityExist.Status = request.Status;
            }

            if (!string.IsNullOrEmpty(request.Approval))
            {
                entityExist.Approval = request.Approval;
            }

            if (request.Status == "ROLLBACK" && entityExist.RollbackCount <= 3)
            {
                entityExist.RollbackCount = entityExist.RollbackCount + 1;
                entityExist.Status = "YET TO SUBMIT";
            }

            if (request.Status == "CANCELLED")
            {
                entityExist.CancellationDateTime = DateTime.Now;
                if (!string.IsNullOrEmpty(request.CancellationConfirmation))
                {
                    entityExist.CancellationConfirmation = request.CancellationConfirmation;
                }
                if (!string.IsNullOrEmpty(request.CancellationReason))
                {
                    entityExist.CancellationReason = request.CancellationReason;
                }
                if (!string.IsNullOrEmpty(request.TravelDeskName))
                {
                    entityExist.TravelDeskName = request.TravelDeskName;
                }
                if (request.TravelDeskId.HasValue)
                {
                    entityExist.TravelDeskId = request.TravelDeskId;
                }                   
            }

            if (!string.IsNullOrEmpty(request.JourneyNumber))
            {
                entityExist.JourneyNumber = request.JourneyNumber;
            }

            _tripRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
