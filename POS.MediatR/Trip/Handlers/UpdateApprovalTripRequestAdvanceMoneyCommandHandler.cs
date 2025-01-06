using AutoMapper;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
    public class UpdateApprovalTripRequestAdvanceMoneyCommandHandler : IRequestHandler<UpdateApprovalTripRequestAdvanceMoneyCommand, ServiceResponse<bool>>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateApprovalTripRequestAdvanceMoneyCommandHandler> _logger;

        public UpdateApprovalTripRequestAdvanceMoneyCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateApprovalTripRequestAdvanceMoneyCommandHandler> logger
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateApprovalTripRequestAdvanceMoneyCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _tripRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(request.RequestAdvanceMoneyStatus))
            {
                entityExist.RequestAdvanceMoneyStatus = request.RequestAdvanceMoneyStatus;
            }
            if (!string.IsNullOrEmpty(request.AdvanceMoneyRemarks))
            {
                entityExist.AdvanceMoneyRemarks = request.AdvanceMoneyRemarks;
            }
            if (request.AdvanceMoneyApprovedAmount.HasValue)
            {
                entityExist.AdvanceMoneyApprovedAmount = request.AdvanceMoneyApprovedAmount;
            }
            if (request.StatusUpdatedBy.HasValue)
            {
                entityExist.RequestAdvanceMoneyStatusBy = request.StatusUpdatedBy;
            }
            if (request.AdvanceAccountApprovedOn.HasValue)
            {
                entityExist.AdvanceAccountApprovedOn = request.AdvanceAccountApprovedOn;
            }
            if (request.AdvanceAccountApprovedBy.HasValue)
            {
                entityExist.AdvanceAccountApprovedBy = request.AdvanceAccountApprovedBy;
            }
            if (request.AdvanceAccountApprovedAmount.HasValue)
            {
                entityExist.AdvanceAccountApprovedAmount = request.AdvanceAccountApprovedAmount;
            }
            if (!string.IsNullOrEmpty(request.AdvanceAccountApprovedStatus))
            {
                entityExist.AdvanceAccountApprovedStatus = request.AdvanceAccountApprovedStatus;
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