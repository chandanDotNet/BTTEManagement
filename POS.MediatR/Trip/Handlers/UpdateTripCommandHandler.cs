using AutoMapper;
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
    public class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, ServiceResponse<bool>>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripCommandHandler> _logger;

        public UpdateTripCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripCommandHandler> logger
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
        {

            //var entityExist = await _tripRepository
            //.All.FirstOrDefaultAsync(c =>  c.Id != request.Id);

            //if (entityExist != null)
            //{
            //    _logger.LogError("Grade Already Exist.");
            //    return ServiceResponse<bool>.Return409("Grade Already Exist.");
            //}
            var entityExist = await _tripRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.TripNo = request.TripNo;
           // entityExist.TripNumber = request.Tripn;
            entityExist.Description = request.Description;
            entityExist.TripType = request.TripType;
            entityExist.Name = request.Name;
            entityExist.PurposeId = request.PurposeId;
            entityExist.TripStarts = request.TripStarts;
            entityExist.TripEnds = request.TripEnds;
            //entityExist.Status = request.Status;
            //entityExist.Approval = request.Approval;
            entityExist.SourceCityId = request.SourceCityId;
            entityExist.DestinationCityId = request.DestinationCityId;
            entityExist.DepartmentId = request.DepartmentId;
            entityExist.MultiCity = request.MultiCity;
            entityExist.ModeOfTrip = request.ModeOfTrip;
            entityExist.IsRequestAdvanceMoney = request.IsRequestAdvanceMoney;
            entityExist.AdvanceMoney = request.AdvanceMoney;

            entityExist.DestinationCityName = request.DestinationCityName;
            entityExist.SourceCityName = request.SourceCityName;
            entityExist.PurposeFor = request.PurposeFor;
            entityExist.DepartmentName = request.DepartmentName;
            entityExist.CompanyAccountId = request.CompanyAccountId;


            _tripRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
