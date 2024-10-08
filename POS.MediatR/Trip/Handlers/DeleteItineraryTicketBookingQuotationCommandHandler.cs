﻿using AutoMapper;
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
    public class DeleteItineraryTicketBookingQuotationCommandHandler : IRequestHandler<DeleteItineraryTicketBookingQuotationCommand,ServiceResponse<bool>>
    {
        private readonly IItineraryTicketBookingQuotationRepository _itineraryTicketBookingQuotationRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteItineraryTicketBookingQuotationCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public DeleteItineraryTicketBookingQuotationCommandHandler(
            IItineraryTicketBookingQuotationRepository itineraryTicketBookingQuotationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteItineraryTicketBookingQuotationCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment, PathHelper pathHelper, ITripItineraryRepository tripItineraryRepository)
        {
            _itineraryTicketBookingQuotationRepository = itineraryTicketBookingQuotationRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _tripItineraryRepository = tripItineraryRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteItineraryTicketBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _itineraryTicketBookingQuotationRepository.FindAsync(request.Id);

            if (entityExist == null)
            {
                _logger.LogError("Quotation does not exists");
                return ServiceResponse<bool>.Return404("Quotation does not exists");
            }

            _itineraryTicketBookingQuotationRepository.Remove(entityExist);

            var itinerary = await _tripItineraryRepository.FindAsync(entityExist.TripItineraryId);

            if (itinerary != null)
            {
                itinerary.IsQuotationUpload= false;
            }
            _tripItineraryRepository.Update(itinerary);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
