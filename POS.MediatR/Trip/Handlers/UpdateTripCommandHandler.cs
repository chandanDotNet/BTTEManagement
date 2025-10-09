using AutoMapper;
using Azure.Core;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using POS.Common.GenericRepository;
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
        private readonly IGroupTripRepository _groupTripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripCommandHandler> _logger;

        public UpdateTripCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripCommandHandler> logger,
           IGroupTripRepository groupTripRepository
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _groupTripRepository = groupTripRepository;

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
            if (!string.IsNullOrEmpty(request.TripNo))
            {
                entityExist.TripNo = request.TripNo;
            }
            if (!string.IsNullOrEmpty(request.Description))
            {
                entityExist.Description = request.Description;
            }
            // entityExist.TripNumber = request.Tripn;
            if (!string.IsNullOrEmpty(request.TripType))
            {
                entityExist.TripType = request.TripType;
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                entityExist.Name = request.Name;
            }
            if(request.PurposeId.HasValue)
            {
                entityExist.PurposeId = request.PurposeId;
            }
            if (request.TripStarts.HasValue)
            {
                entityExist.TripStarts = request.TripStarts.Value;
            }
            if (request.TripEnds.HasValue)
            {
                entityExist.TripEnds = request.TripEnds.Value;
            }
            if (request.SourceCityId.HasValue)
            {
                entityExist.SourceCityId = request.SourceCityId;
            }
            if (request.DestinationCityId.HasValue)
            {
                entityExist.DestinationCityId = request.DestinationCityId;
            }


            //entityExist.Status = request.Status;
            //entityExist.Approval = request.Approval;

            if (request.DepartmentId.HasValue)
            {
                entityExist.DepartmentId = request.DepartmentId;
            }
            if (!string.IsNullOrEmpty(request.MultiCity))
            {
                entityExist.MultiCity = request.MultiCity;
            }
            if (!string.IsNullOrEmpty(request.ModeOfTrip))
            {
                entityExist.ModeOfTrip = request.ModeOfTrip;
            }
            if (!string.IsNullOrEmpty(request.DestinationCityName))
            {
                entityExist.DestinationCityName = request.DestinationCityName;
            }
            if (!string.IsNullOrEmpty(request.SourceCityName))
            {
                entityExist.SourceCityName = request.SourceCityName;
            }
            if (!string.IsNullOrEmpty(request.PurposeFor))
            {
                entityExist.PurposeFor = request.PurposeFor;
            }
            if (!string.IsNullOrEmpty(request.DepartmentName))
            {
                entityExist.DepartmentName = request.DepartmentName;
            }

            if (request.CompanyAccountId.HasValue)
            {
                entityExist.CompanyAccountId = request.CompanyAccountId;
            }
            if (!string.IsNullOrEmpty(request.VendorCode))
            {
                entityExist.VendorCode = request.VendorCode;
            }
            if (request.IsGroupTrip == true)
            {
                entityExist.IsGroupTrip = request.IsGroupTrip;
            }
              
            if (!string.IsNullOrEmpty(request.NoOfPerson))
            {
                entityExist.NoOfPerson = request.NoOfPerson;
            }
            if (request.Consent==true)
            {
                entityExist.Consent = request.Consent;
            }
                //entityExist.IsRequestAdvanceMoney = request.IsRequestAdvanceMoney;
                //entityExist.AdvanceMoney = request.AdvanceMoney;
             


            _tripRepository.Update(entityExist);

           

            if (request.GroupTrips != null)
            {
                var groupTripExist = await _groupTripRepository.All.Where(v => v.TripId == request.Id).ToListAsync();
                if (groupTripExist.Count > 0)
                {
                    _groupTripRepository.RemoveRange(groupTripExist);
                }

                request.GroupTrips.ForEach(item =>
                {
                    item.TripId = request.Id;
                    item.Id = Guid.NewGuid();
                });

                var groupTrip = _mapper.Map<List<GroupTrip>>(request.GroupTrips);
                _groupTripRepository.AddRange(groupTrip);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
