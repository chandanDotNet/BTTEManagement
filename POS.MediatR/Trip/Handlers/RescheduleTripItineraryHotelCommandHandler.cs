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

        public RescheduleTripItineraryHotelCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<RescheduleTripItineraryHotelCommandHandler> logger
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(RescheduleTripItineraryHotelCommand request, CancellationToken cancellationToken)
        {

            var entityExist =  _tripItineraryRepository.FindBy(v => v.Id == request.Id).FirstOrDefault();
            if (entityExist != null)
            {
               
                if(request.RescheduleDepartureDate.HasValue)
                {
                    entityExist.RescheduleDepartureDate = request.RescheduleDepartureDate;
                }
                if (!string.IsNullOrWhiteSpace(request.RescheduleReason))
                {
                    entityExist.RescheduleReason = request.RescheduleReason;
                }                
                entityExist.IsReschedule = request.IsReschedule;
                entityExist.ApprovalStatus = "PENDING";
                entityExist.BookStatus = "RESCHEDULE";
            }
         
            _tripItineraryRepository.Update(entityExist);

               
            

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
