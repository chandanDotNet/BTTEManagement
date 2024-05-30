using AutoMapper;
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
    public class UpdateTripHotelBookingCommandHandler : IRequestHandler<UpdateTripHotelBookingCommand, ServiceResponse<bool>>
    {

        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripHotelBookingCommandHandler> _logger;

        public UpdateTripHotelBookingCommandHandler(
           ITripHotelBookingRepository tripHotelBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripHotelBookingCommandHandler> logger
          )
        {
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripHotelBookingCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.tripHotelBooking)
            {
                var entityExist = await _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefaultAsync();
                entityExist.TripId = tv.TripId;
                entityExist.CheckIn = tv.CheckIn;
                entityExist.BookTypeBy = tv.BookTypeBy;
                entityExist.CheckOut = tv.CheckOut;
                entityExist.CheckInTime = tv.CheckInTime;
                entityExist.CheckOutTime = tv.CheckOutTime;
                entityExist.CityId = tv.CityId;
                entityExist.TentativeAmount = tv.TentativeAmount;
                entityExist.CityName = tv.CityName;
                entityExist.NearbyLocation = tv.NearbyLocation;
                entityExist.PreferredHotel = tv.PreferredHotel;

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
