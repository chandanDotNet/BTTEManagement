using AutoMapper;
using BTTEM.Data;
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
    public class CancelTripItineraryHotelCommandHandler : IRequestHandler<CancelTripItineraryHotelCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelTripItineraryHotelCommandHandler> _logger;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;

        public CancelTripItineraryHotelCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<CancelTripItineraryHotelCommandHandler> logger,
           ITripHotelBookingRepository tripHotelBookingRepository
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _tripHotelBookingRepository = tripHotelBookingRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(CancelTripItineraryHotelCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.cancelTripItineraryHotel)
            {
                if (tv.IsItinerary == true)
                {
                    var entityExist = _tripItineraryRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                    if (entityExist != null)
                    {
                        entityExist.ApprovalStatus = "CANCEL REQUEST";
                        if (!string.IsNullOrWhiteSpace(tv.NoOfTickets))
                        {
                            entityExist.NoOfTickets = tv.NoOfTickets;
                        }                    

                    }

                    _tripItineraryRepository.Update(entityExist);
                }
                else
                {
                    var entityExist = _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                    if (entityExist != null)
                    {
                        entityExist.ApprovalStatus = "CANCEL REQUEST";
                        if (!string.IsNullOrWhiteSpace(tv.NoOfRoom))
                        {
                            entityExist.NoOfRoom = tv.NoOfRoom;
                        }
                       
                    }

                    _tripHotelBookingRepository.Update(entityExist);                  
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
