using AutoMapper;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
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
    public class UpdateItineraryTicketBookingIsAvailCommandHandler : IRequestHandler<UpdateItineraryTicketBookingIsAvailCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateItineraryTicketBookingIsAvailCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public UpdateItineraryTicketBookingIsAvailCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IItineraryTicketBookingRepository itineraryTicketBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateItineraryTicketBookingIsAvailCommandHandler> logger,
           IWebHostEnvironment webHostEnvironment

          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _itineraryTicketBookingRepository = itineraryTicketBookingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;


        }

        public async Task<ServiceResponse<bool>> Handle(UpdateItineraryTicketBookingIsAvailCommand request, CancellationToken cancellationToken)
        {
            var entityExistList = await _itineraryTicketBookingRepository.FindAsync(request.Id.FirstOrDefault());
            if(entityExistList != null)
            {
                var ItineraryId = entityExistList.TripItineraryId;

                var listData=  _itineraryTicketBookingRepository.FindBy(c=>c.TripItineraryId== ItineraryId).ToList();
                listData.ToList().ForEach(item => { item.IsAvail = false; });
                _itineraryTicketBookingRepository.UpdateRange(listData);
            }

            foreach (var Item in request.Id)
            {
                var entityExist = await _itineraryTicketBookingRepository.FindAsync(Item);
                entityExist.IsAvail = true;
                _itineraryTicketBookingRepository.Update(entityExist);
            }

            //var entityExist = await _itineraryTicketBookingRepository.FindAsync(request.Id);
            //if (entityExist == null)
            //{
            //    _logger.LogError("Trip Itinerary Ticket Does not exists");
            //    return ServiceResponse<bool>.Return404("Trip Itinerary Ticket  Does not exists");
            //}
            

           // _itineraryTicketBookingRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
