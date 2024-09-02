using AutoMapper;
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
    public class UpdateItineraryHotelBookingQuotationCommandHandler : IRequestHandler<UpdateItineraryHotelBookingQuotationCommand, ServiceResponse<bool>>
    {
        private readonly IItineraryHotelBookingQuotationRepository _itineraryHotelBookingQuotationRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateItineraryHotelBookingQuotationCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public UpdateItineraryHotelBookingQuotationCommandHandler(
            IItineraryHotelBookingQuotationRepository itineraryHotelBookingQuotationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateItineraryHotelBookingQuotationCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment, PathHelper pathHelper, ITripHotelBookingRepository tripHotelBookingRepository)
        {
            _itineraryHotelBookingQuotationRepository = itineraryHotelBookingQuotationRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _tripHotelBookingRepository = tripHotelBookingRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateItineraryHotelBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _itineraryHotelBookingQuotationRepository.FindAsync(request.Id);

            if (entityExist == null)
            {
                _logger.LogError("Quotation does not exists");
                return ServiceResponse<bool>.Return404("Quotation does not exists");
            }
            if (!string.IsNullOrEmpty(request.QuotationName))
            {
                entityExist.QuotationName = request.QuotationName;
            }

            if (!string.IsNullOrEmpty(request.TravelDeskNotes))
            {
                entityExist.TravelDeskNotes = request.TravelDeskNotes;
            }

            if (!string.IsNullOrEmpty(request.RMNotes))
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

            _itineraryHotelBookingQuotationRepository.Update(entityExist);

            var _tripHotelBooking = await _tripHotelBookingRepository.FindAsync(request.ItineraryHotelId);

            _tripHotelBooking.IsQuotationUpload = request.IsQuotationUpload;

            _tripHotelBookingRepository.Update(_tripHotelBooking);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
