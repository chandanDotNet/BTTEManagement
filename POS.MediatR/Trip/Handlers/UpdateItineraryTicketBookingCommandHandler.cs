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
    public class UpdateItineraryTicketBookingCommandHandler : IRequestHandler<UpdateItineraryTicketBookingCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateItineraryTicketBookingCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        public UpdateItineraryTicketBookingCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IItineraryTicketBookingRepository itineraryTicketBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateItineraryTicketBookingCommandHandler> logger,
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

        public async Task<ServiceResponse<bool>> Handle(UpdateItineraryTicketBookingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _itineraryTicketBookingRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Trip Itinerary Ticket Does not exists");
                return ServiceResponse<bool>.Return404("Trip Itinerary Ticket  Does not exists");
            }
            if (!string.IsNullOrWhiteSpace(request.CancelationReason))
            {
                entityExist.CancelationReason = request.CancelationReason;
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                entityExist.Status = request.Status;
            }
            if (!string.IsNullOrWhiteSpace(request.VendorName))
            {
                entityExist.VendorName = request.VendorName;
            }
            if (!string.IsNullOrWhiteSpace(request.BookingDate))
            {
                entityExist.BookingDate = request.BookingDate;
            }
            if (!string.IsNullOrWhiteSpace(request.BookingTime))
            {
                entityExist.BookingTime = request.BookingTime;
            }
            if (request.BookingAmount>0)
            {
                entityExist.BookingAmount = request.BookingAmount;
            }
            if (request.AgentCharge > 0)
            {
                entityExist.AgentCharge = request.AgentCharge;
            }
            if (request.CancelationCharge > 0)
            {
                entityExist.CancelationCharge = request.CancelationCharge;
            }
            if (request.TotalAmount > 0)
            {
                entityExist.TotalAmount = request.TotalAmount;
            }
            entityExist.IsAvail=request.IsAvail;

            _itineraryTicketBookingRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
