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
    public class AddItineraryHotelBookingQuotationCommandHandler : IRequestHandler<AddItineraryHotelBookingQuotationCommand, ServiceResponse<List<ItineraryHotelBookingQuotationDto>>>
    {
        private readonly IItineraryHotelBookingQuotationRepository _itineraryHotelBookingQuotationRepository;
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
            IItineraryHotelBookingQuotationRepository itineraryHotelBookingQuotationRepository
            )
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _itineraryHotelBookingQuotationRepository = itineraryHotelBookingQuotationRepository;
        }

        public async Task<ServiceResponse<List<ItineraryHotelBookingQuotationDto>>> Handle(AddItineraryHotelBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<List<ItineraryHotelBookingQuotation>>(request.ItineraryHotelBookingQuotationList);
            entity.ForEach(item =>
            {
                item.Id = Guid.NewGuid();
            });

            //================== Quotation Upload
            int i = 0;
            request.ItineraryHotelBookingQuotationList.ForEach(async item =>
            {
                if (!string.IsNullOrWhiteSpace(item.QuotationName) && !string.IsNullOrWhiteSpace(item.QuotationPath))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.ItineraryTicketBookingQuotationAttachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(item.QuotationName);
                    var id = Guid.NewGuid();
                    var path = $"{id}.{extension}";
                    var documentPath = Path.Combine(pathToSave, path);
                    string base64 = item.QuotationPath.Split(',').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(base64))
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        try
                        {
                            await File.WriteAllBytesAsync($"{documentPath}", bytes);
                            entity[i].QuotationPath = path;
                            i++;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entity);
                        }
                    }
                }
            });

            _itineraryHotelBookingQuotationRepository.AddRange(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Itinerary");
                return ServiceResponse<List<ItineraryHotelBookingQuotationDto>>.Return500();
            }

            var tripItineraryQuotation = _mapper.Map<List<ItineraryHotelBookingQuotationDto>>(entity);
            return ServiceResponse<List<ItineraryHotelBookingQuotationDto>>.ReturnResultWith200(tripItineraryQuotation);
        }
    }
}
