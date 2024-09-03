using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class AddItineraryHotelBookingQuotationCommandHandler : IRequestHandler<AddItineraryHotelBookingQuotationCommand, ServiceResponse<ItineraryHotelBookingQuotationDto>>
    {
        private readonly IItineraryHotelBookingQuotationRepository _itineraryHotelBookingQuotationRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddItineraryHotelBookingQuotationCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public AddItineraryHotelBookingQuotationCommandHandler(
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddItineraryHotelBookingQuotationCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IItineraryHotelBookingQuotationRepository itineraryHotelBookingQuotationRepository,
            ITripHotelBookingRepository tripHotelBookingRepository
            )
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _itineraryHotelBookingQuotationRepository = itineraryHotelBookingQuotationRepository;
            _tripHotelBookingRepository = tripHotelBookingRepository;
        }

        public async Task<ServiceResponse<ItineraryHotelBookingQuotationDto>> Handle(AddItineraryHotelBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ItineraryHotelBookingQuotation>(request);
            entity.Id = Guid.NewGuid();

            //================== Quotation Upload

            if (!string.IsNullOrWhiteSpace(entity.QuotationName) && !string.IsNullOrWhiteSpace(entity.QuotationPath))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.ItineraryTicketBookingQuotationAttachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(entity.QuotationName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = entity.QuotationPath.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.QuotationPath = path;

                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            _itineraryHotelBookingQuotationRepository.Add(entity);

            var _tripHotelBooking = await _tripHotelBookingRepository.FindAsync(request.TripHotelBookingId);

            _tripHotelBooking.RMStatus = request.RMStatus;

            _tripHotelBookingRepository.Update(_tripHotelBooking);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Itinerary");
                return ServiceResponse<ItineraryHotelBookingQuotationDto>.Return500();
            }

            var tripItineraryQuotation = _mapper.Map<ItineraryHotelBookingQuotationDto>(entity);
            return ServiceResponse<ItineraryHotelBookingQuotationDto>.ReturnResultWith200(tripItineraryQuotation);
        }
    }
}
