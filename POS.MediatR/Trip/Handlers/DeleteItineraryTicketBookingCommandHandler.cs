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
    public class DeleteItineraryTicketBookingCommandHandler : IRequestHandler<DeleteItineraryTicketBookingCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteItineraryTicketBookingCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DeleteItineraryTicketBookingCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IItineraryTicketBookingRepository itineraryTicketBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteItineraryTicketBookingCommandHandler> logger,
           IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _itineraryTicketBookingRepository = itineraryTicketBookingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;

        }

        public async Task<ServiceResponse<bool>> Handle(DeleteItineraryTicketBookingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _itineraryTicketBookingRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Trip Itinerary Ticket Does not exists");
                return ServiceResponse<bool>.Return404("Trip Itinerary Ticket  Does not exists");
            }

            
            _itineraryTicketBookingRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
