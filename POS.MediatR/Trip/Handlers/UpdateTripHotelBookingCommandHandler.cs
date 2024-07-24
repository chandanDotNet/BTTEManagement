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
                if (tv.Id == Guid.Empty)
                {
                    var entity = _mapper.Map<Data.TripHotelBooking>(tv);
                    entity.Id = Guid.NewGuid();
                    entity.ApprovalStatus = "PENDING";

                    _tripHotelBookingRepository.Add(entity);
                }
                else
                {
                    var entityExist = await _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefaultAsync();
                    if (entityExist != null)
                    {


                        entityExist.TripId = tv.TripId;
                        if (tv.CheckIn.HasValue)
                        {
                            entityExist.CheckIn = (DateTime)tv.CheckIn;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.BookTypeBy))
                        {
                            entityExist.BookTypeBy = tv.BookTypeBy;
                        }
                        if (tv.CheckOut.HasValue)
                        {
                            entityExist.CheckOut = (DateTime)tv.CheckOut;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CheckInTime))
                        {
                            entityExist.CheckInTime = tv.CheckInTime;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CheckOutTime))
                        {
                            entityExist.CheckOutTime = tv.CheckOutTime;
                        }
                        if (tv.CityId.HasValue)
                        {
                            entityExist.CityId = tv.CityId;
                        }
                        if (tv.TentativeAmount > 0)
                        {
                            entityExist.TentativeAmount = tv.TentativeAmount;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CityName))
                        {
                            entityExist.CityName = tv.CityName;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.NearbyLocation))
                        {
                            entityExist.NearbyLocation = tv.NearbyLocation;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.PreferredHotel))
                        {
                            entityExist.PreferredHotel = tv.PreferredHotel;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.BookStatus))
                        {
                            entityExist.BookStatus = tv.BookStatus;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.ApprovalStatus))
                        {
                            entityExist.ApprovalStatus = tv.ApprovalStatus;
                        }
                        if (tv.CancelationCharge > 0)
                        {
                            entityExist.CancelationCharge = tv.CancelationCharge;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CancelationReason))
                        {
                            entityExist.CancelationReason = tv.CancelationReason;
                        }
                        if (tv.CancelationDate.HasValue)
                        {
                            entityExist.CancelationDate = tv.CancelationDate;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.NoOfRoom))
                        {
                            entityExist.NoOfRoom = tv.NoOfRoom;
                        }

                        _tripHotelBookingRepository.Update(entityExist);
                    }
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
