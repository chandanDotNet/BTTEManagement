using AutoMapper;
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
    public class UpdateTripItineraryBookStatusCommandHandler : IRequestHandler<UpdateTripItineraryBookStatusCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripItineraryBookStatusCommandHandler> _logger;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;

        public UpdateTripItineraryBookStatusCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripItineraryBookStatusCommandHandler> logger,
            ITripHotelBookingRepository tripHotelBookingRepository
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _tripHotelBookingRepository = tripHotelBookingRepository;


        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripItineraryBookStatusCommand request, CancellationToken cancellationToken)
        {

            if (request.IsItinerary == true)
            {

                var entityExist = await _tripItineraryRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
                entityExist.BookStatus = request.BookStatus;

                if (!request.ExpenseId.HasValue || request.ExpenseId.Value == Guid.Empty)
                {
                    //not valid GUID
                }
                else
                {
                    entityExist.ExpenseId = request.ExpenseId;
                }

                _tripItineraryRepository.Update(entityExist);
            }
            else
            {
                var entityExist = await _tripHotelBookingRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
                entityExist.BookStatus = request.BookStatus;
                if (!request.ExpenseId.HasValue || request.ExpenseId.Value == Guid.Empty)
                {
                    //not valid GUID
                }
                else
                {
                    entityExist.ExpenseId = request.ExpenseId;
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
