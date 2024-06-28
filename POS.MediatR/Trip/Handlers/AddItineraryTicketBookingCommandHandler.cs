using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto;

using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;
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
using PathHelper = POS.Helper.PathHelper;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class AddItineraryTicketBookingCommandHandler : IRequestHandler<AddItineraryTicketBookingCommand, ServiceResponse<ItineraryTicketBookingDto>>
    {
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddItineraryTicketBookingCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddItineraryTicketBookingCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IItineraryTicketBookingRepository itineraryTicketBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddItineraryTicketBookingCommandHandler> logger,
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

        public async Task<ServiceResponse<ItineraryTicketBookingDto>> Handle(AddItineraryTicketBookingCommand request, CancellationToken cancellationToken)
        {
                        
                var entity = _mapper.Map<ItineraryTicketBooking>(request);
                entity.Id = Guid.NewGuid();
                entity.Status = "PENDING";

            
            //==================  Ticket Upload

            if (!string.IsNullOrWhiteSpace(request.TicketReceiptName) && !string.IsNullOrWhiteSpace(request.TicketDocumentData))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.TicketReceiptName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.TicketDocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.TicketReceiptPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            //==================  Approval Document Upload

            if (!string.IsNullOrWhiteSpace(request.ApprovalDocumentsReceiptName) && !string.IsNullOrWhiteSpace(request.ApprovalDocumentData))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.ApprovalDocumentsReceiptName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.ApprovalDocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.ApprovalDocumentsReceiptPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            //=================

            _itineraryTicketBookingRepository.Add(entity);
            var itineraryData = await _tripItineraryRepository.FindAsync(request.TripItineraryId);
            if(itineraryData != null) {

                itineraryData.BookStatus = "BOOKED";
                _tripItineraryRepository.Update(itineraryData);
            }

            // var dd = _uow.SaveAsync();
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Itinerary");
                return ServiceResponse<ItineraryTicketBookingDto>.Return500();
            }
            //var entityRes = await _itineraryTicketBookingRepository.AllIncluding(c => c.ArrivalCity, b => b.DepartureCity).ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var tripItinerary = _mapper.Map<ItineraryTicketBookingDto>(entity);
            return ServiceResponse<ItineraryTicketBookingDto>.ReturnResultWith200(tripItinerary);
        }

    }
}
