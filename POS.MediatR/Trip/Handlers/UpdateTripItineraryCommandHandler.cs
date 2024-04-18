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
    public class UpdateTripItineraryCommandHandler : IRequestHandler<UpdateTripItineraryCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripItineraryCommandHandler> _logger;

        public UpdateTripItineraryCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripItineraryCommandHandler> logger
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripItineraryCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.TripItinerary)
            {
                var entityExist = await _tripItineraryRepository.FindBy(v => v.Id == tv.Id).FirstOrDefaultAsync();
                entityExist.TripId = tv.TripId;
                entityExist.TripBy = tv.TripBy;
                entityExist.BookTypeBy = tv.BookTypeBy;
                entityExist.TripWayType = tv.TripWayType;
                entityExist.DepartureCityId = tv.DepartureCityId;
                entityExist.ArrivalCityId = tv.ArrivalCityId;             
                entityExist.DepartureDate = tv.DepartureDate;
                entityExist.TripPreference1 = tv.TripPreference1;
                entityExist.TripPreference2 = tv.TripPreference2;
                entityExist.TripPreferenceTime = tv.TripPreferenceTime;
                entityExist.TripReturnPreferenceTime = tv.TripReturnPreferenceTime;
                entityExist.TripPreferenceSeat = tv.TripPreferenceSeat;
                entityExist.ReturnDate = tv.ReturnDate;                

                _tripItineraryRepository.Update(entityExist);
            }

           
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
