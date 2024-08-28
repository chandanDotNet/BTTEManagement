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
    public class AddItineraryTicketBookingQuotationCommandHandler : IRequestHandler<AddItineraryTicketBookingQuotationCommand, ServiceResponse<List<ItineraryTicketBookingQuotationDto>>>
    {
        private readonly IItineraryTicketBookingQuotationRepository _itineraryTicketBookingQuotationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddItineraryTicketBookingQuotationCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public AddItineraryTicketBookingQuotationCommandHandler(
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddItineraryTicketBookingQuotationCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IItineraryTicketBookingQuotationRepository itineraryTicketBookingQuotationRepository
            )
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _itineraryTicketBookingQuotationRepository = itineraryTicketBookingQuotationRepository;
        }
        public async Task<ServiceResponse<List<ItineraryTicketBookingQuotationDto>>> Handle(AddItineraryTicketBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<List<ItineraryTicketBookingQuotation>>(request.ItineraryTicketBookingQuotationList);
            entity.ForEach(item =>
            {
                item.Id = Guid.NewGuid();
            });

            //================== Quotation Upload
            int i = 0;
            request.ItineraryTicketBookingQuotationList.ForEach(async item =>
            {
                if (!string.IsNullOrWhiteSpace(item.QuotaionName) && !string.IsNullOrWhiteSpace(item.QuotaionPath))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.ItineraryTicketBookingQuotationAttachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(item.QuotaionName);
                    var id = Guid.NewGuid();
                    var path = $"{id}.{extension}";
                    var documentPath = Path.Combine(pathToSave, path);
                    string base64 = item.QuotaionPath.Split(',').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(base64))
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        try
                        {
                            await File.WriteAllBytesAsync($"{documentPath}", bytes);
                            entity[i].QuotaionPath = path;
                            i++;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entity);
                        }
                    }
                }
            });            

            _itineraryTicketBookingQuotationRepository.AddRange(entity);        
           
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Itinerary");
                //return ServiceResponse<ItineraryTicketBookingQuotationDto>.Return500();
            }

            var tripItineraryQuotation = _mapper.Map<List<ItineraryTicketBookingQuotationDto>>(entity);
            return ServiceResponse<List<ItineraryTicketBookingQuotationDto>>.ReturnResultWith200(tripItineraryQuotation);
        }
    }
}
