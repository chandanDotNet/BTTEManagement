﻿using AutoMapper;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class UpdateTripItineraryCommandHandler : IRequestHandler<UpdateTripItineraryCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripItineraryCommandHandler> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserInfoToken _userInfoToken;

        public UpdateTripItineraryCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripItineraryCommandHandler> logger,
           IUserRepository userRepository,
           UserInfoToken userInfoToken
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _userInfoToken = userInfoToken;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripItineraryCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
            foreach (var tv in request.TripItinerary)
            {
                tv.Id = Guid.NewGuid();
                var entity = _mapper.Map<Data.TripItinerary>(tv);
                entity.ApprovalStatus = "PENDING";

                if (userDetails.IsDirector)
                {
                    entity.ApprovalStatus = "APPROVED";
                    entity.ApprovalStatusDate = DateTime.Now;
                }

                _tripItineraryRepository.Add(entity);

                //if (tv.Id == Guid.Empty)
                //{
                //    tv.Id = Guid.NewGuid();
                //    var entity = _mapper.Map<Data.TripItinerary>(tv);
                //    entity.ApprovalStatus = "PENDING";
                //    _tripItineraryRepository.Add(entity);
                //}
                //else
                //{
                //    var entityExist = await _tripItineraryRepository.FindBy(v => v.Id == tv.Id).FirstOrDefaultAsync();
                //    if (entityExist != null)
                //    {
                //        entityExist.TripId = tv.TripId;
                //        entityExist.TripBy = tv.TripBy;
                //        entityExist.BookTypeBy = tv.BookTypeBy;
                //        entityExist.TripWayType = tv.TripWayType;
                //        entityExist.DepartureCityId = tv.DepartureCityId;
                //        entityExist.ArrivalCityId = tv.ArrivalCityId;
                //        entityExist.DepartureDate = tv.DepartureDate;
                //        entityExist.TripPreference1 = tv.TripPreference1;
                //        entityExist.TripPreference2 = tv.TripPreference2;
                //        entityExist.TripPreferenceTime = tv.TripPreferenceTime;
                //        entityExist.TripReturnPreferenceTime = tv.TripReturnPreferenceTime;
                //        entityExist.TripPreferenceSeat = tv.TripPreferenceSeat;
                //        entityExist.ReturnDate = tv.ReturnDate;
                //        entityExist.TentativeAmount = tv.TentativeAmount;
                //        entityExist.BookStatus = tv.BookStatus;
                //        entityExist.ExpenseId = tv.ExpenseId;

                //        entityExist.DepartureCityName = tv.DepartureCityName;
                //        entityExist.ArrivalCityName = tv.ArrivalCityName;
                //        entityExist.TrainClass = tv.TrainClass;
                //        entityExist.PreferredTrain = tv.PreferredTrain;
                //        entityExist.PickupTime = tv.PickupTime;
                //        entityExist.BusType = tv.BusType;
                //        entityExist.CarType = tv.CarType;
                //        entityExist.NoOfTickets = tv.NoOfTickets;
                //        _tripItineraryRepository.Update(entityExist);
                //    }
                    
                //}
            }
            
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
