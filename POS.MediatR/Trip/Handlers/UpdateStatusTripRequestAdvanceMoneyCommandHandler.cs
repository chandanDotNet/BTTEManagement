﻿using AutoMapper;
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
    public class UpdateStatusTripRequestAdvanceMoneyCommandHandler : IRequestHandler<UpdateStatusTripRequestAdvanceMoneyCommand, ServiceResponse<bool>>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateStatusTripRequestAdvanceMoneyCommandHandler> _logger;

        public UpdateStatusTripRequestAdvanceMoneyCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateStatusTripRequestAdvanceMoneyCommandHandler> logger
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateStatusTripRequestAdvanceMoneyCommand request, CancellationToken cancellationToken)
        {

            var entityExist = await _tripRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
           
            entityExist.RequestAdvanceMoneyDate = request.RequestAdvanceMoneyDate;
            entityExist.RequestAdvanceMoneyStatus = request.Status;
            entityExist.RequestAdvanceMoneyStatusBy = request.StatusUpdatedBy;

            _tripRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
