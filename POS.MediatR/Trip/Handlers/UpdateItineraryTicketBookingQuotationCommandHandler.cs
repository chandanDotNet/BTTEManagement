using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class UpdateItineraryTicketBookingQuotationCommandHandler : IRequestHandler<UpdateItineraryTicketBookingQuotationCommand, ServiceResponse<bool>>
    {
        private readonly IItineraryTicketBookingQuotationRepository _itineraryTicketBookingQuotationRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateItineraryTicketBookingQuotationCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public UpdateItineraryTicketBookingQuotationCommandHandler(
            IItineraryTicketBookingQuotationRepository itineraryTicketBookingQuotationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateItineraryTicketBookingQuotationCommandHandler> logger,
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

        public async Task<ServiceResponse<bool>> Handle(UpdateItineraryTicketBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _itineraryTicketBookingQuotationRepository.FindAsync(request.Id);

            if (entityExist == null)
            {
                _logger.LogError("Quotation does not exists");
                return ServiceResponse<bool>.Return404("Quotation does not exists");
            }
            if (!string.IsNullOrWhiteSpace(request.QuotationName))
            {
                entityExist.QuotationName = request.QuotationName;
            }
            if (!string.IsNullOrWhiteSpace(request.TravelDeskNotes))
            {
                entityExist.TravelDeskNotes = request.TravelDeskNotes;

            }
            if (!string.IsNullOrWhiteSpace(request.RMNotes))
            {
                entityExist.RMNotes = request.RMNotes;
            }

            if (!string.IsNullOrWhiteSpace(request.QuotationName) && !string.IsNullOrWhiteSpace(request.QuotationPath))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.ItineraryTicketBookingQuotationAttachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.QuotationName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.QuotationPath.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entityExist.QuotationPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entityExist);
                    }
                }
            }

            var tripItinerary = await _tripItineraryRepository.FindAsync(request.TripItineraryId);

            if (!string.IsNullOrWhiteSpace(request.RMStatus))
            {
                tripItinerary.RMStatus = request.RMStatus;

                _tripItineraryRepository.Update(tripItinerary);
            }

            _itineraryTicketBookingQuotationRepository.Update(entityExist);           

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
