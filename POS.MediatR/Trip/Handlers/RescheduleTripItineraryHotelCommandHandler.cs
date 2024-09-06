using AutoMapper;
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
    public class RescheduleTripItineraryHotelCommandHandler : IRequestHandler<RescheduleTripItineraryHotelCommand, ServiceResponse<bool>>
    {
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<RescheduleTripItineraryHotelCommandHandler> _logger;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;

        public RescheduleTripItineraryHotelCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<RescheduleTripItineraryHotelCommandHandler> logger,
           ITripHotelBookingRepository tripHotelBookingRepository
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _tripHotelBookingRepository = tripHotelBookingRepository;

        }

        public async Task<ServiceResponse<bool>> Handle(RescheduleTripItineraryHotelCommand request, CancellationToken cancellationToken)
        {
            if (request.IsItinerary == true)
            {
                var entityExist = _tripItineraryRepository.FindBy(v => v.Id == request.Id).FirstOrDefault();
                if (entityExist != null)
                {

                    if (request.ItineraryRescheduleDepartureDate.HasValue)
                    {
                        entityExist.RescheduleDepartureDate = request.ItineraryRescheduleDepartureDate;
                    }
                    if (!string.IsNullOrWhiteSpace(request.RescheduleReason))
                    {
                        entityExist.RescheduleReason = request.RescheduleReason;
                    }
                    entityExist.IsReschedule = request.IsReschedule;
                    entityExist.ApprovalStatus = "RESCHEDULE REQUEST";
                   // entityExist.BookStatus = "RESCHEDULE";
                }

                _tripItineraryRepository.Update(entityExist);

            }
            else
            {
                var entityExist = _tripHotelBookingRepository.FindBy(v => v.Id == request.Id).FirstOrDefault();
                if (entityExist != null)
                {
                    if (request.RescheduleCheckIn.HasValue)
                    {
                        entityExist.RescheduleCheckIn = (DateTime)request.RescheduleCheckIn;
                    }
                    if (request.RescheduleCheckOut.HasValue)
                    {
                        entityExist.RescheduleCheckOut = (DateTime)request.RescheduleCheckOut;
                    }
                    if (!string.IsNullOrWhiteSpace(request.RescheduleCheckInTime))
                    {
                        entityExist.RescheduleCheckInTime = request.RescheduleCheckInTime;
                    }
                    if (!string.IsNullOrWhiteSpace(request.RescheduleCheckOutTime))
                    {
                        entityExist.RescheduleCheckOutTime = request.RescheduleCheckOutTime;
                    }
                    if (!string.IsNullOrWhiteSpace(request.RescheduleReason))
                    {
                        entityExist.RescheduleReason = request.RescheduleReason;
                    }
                    if (request.IsRescheduleChargePlus.HasValue)
                    {
                        entityExist.IsRescheduleChargePlus = request.IsRescheduleChargePlus;
                    }
                    entityExist.IsReschedule = request.IsReschedule;
                    entityExist.ApprovalStatus = "RESCHEDULE REQUEST";
                    //entityExist.BookStatus = "RESCHEDULE";
                }
                _tripHotelBookingRepository.Update(entityExist);
            }


            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
