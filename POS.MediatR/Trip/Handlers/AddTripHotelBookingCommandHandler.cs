using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
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
    public class AddTripHotelBookingCommandHandler : IRequestHandler<AddTripHotelBookingCommand, ServiceResponse<TripHotelBookingDto>>
    {

        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTripHotelBookingCommandHandler> _logger;

        public AddTripHotelBookingCommandHandler(
           ITripHotelBookingRepository tripHotelBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddTripHotelBookingCommandHandler> logger
          )
        {
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<TripHotelBookingDto>> Handle(AddTripHotelBookingCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.tripHotelBooking)
            {
                var entity = _mapper.Map<Data.TripHotelBooking>(tv);
                entity.Id = Guid.NewGuid();
                entity.ApprovalStatus = "PENDING";
                entity.TotalAmount = (tv.Amount == null ? 0 : tv.Amount) + (tv.AgentCharge == null ? 0 : tv.AgentCharge) + (tv.BookingAmount == null ? 0 : tv.BookingAmount);
                _tripHotelBookingRepository.Add(entity);
            }

            //var entity = _mapper.Map<BTTEM.Data.TripItinerary>(request);            

            //    var id = Guid.NewGuid();
            //    entity.Id = id;

            //_tripItineraryRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Hotel Booking");
                return ServiceResponse<TripHotelBookingDto>.Return500();
            }
            var entityRes = await _tripHotelBookingRepository.AllIncluding(c => c.City).ProjectTo<TripHotelBookingDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var tripHotelBooking = _mapper.Map<TripHotelBookingDto>(entityRes);
            return ServiceResponse<TripHotelBookingDto>.ReturnResultWith200(tripHotelBooking);
        }

    }
}
