using AutoMapper;
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
    public class UpdateItineraryTicketBookingCommandHandler : IRequestHandler<UpdateItineraryTicketBookingCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateItineraryTicketBookingCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;


        public UpdateItineraryTicketBookingCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IItineraryTicketBookingRepository itineraryTicketBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateItineraryTicketBookingCommandHandler> logger,
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
            if (request.VendorId.HasValue)
            {
                entityExist.VendorId = request.VendorId;
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
            entityExist.IsReschedule=request.IsReschedule;
            if (!string.IsNullOrWhiteSpace(request.RescheduleStatus))
            {
                entityExist.RescheduleStatus = request.RescheduleStatus;
            }
            if (!string.IsNullOrWhiteSpace(request.RescheduleReason))
            {
                entityExist.RescheduleReason = request.RescheduleReason;
            }
            if (request.RescheduleCharge > 0)
            {
                entityExist.RescheduleCharge = request.RescheduleCharge;
            }
            if (!string.IsNullOrWhiteSpace(request.RescheduleBookingDate))
            {
                entityExist.RescheduleBookingDate = request.RescheduleBookingDate;
            }
            if (!string.IsNullOrWhiteSpace(request.RescheduleBookingTime))
            {
                entityExist.RescheduleBookingTime = request.RescheduleBookingTime;
            }

            //==================  Ticket Upload

            if (!string.IsNullOrWhiteSpace(request.RescheduleTicketReceiptName) && !string.IsNullOrWhiteSpace(request.RescheduleTicketDocumentData))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.RescheduleTicketReceiptName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.RescheduleTicketDocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entityExist.RescheduleTicketReceiptPath = path;
                        entityExist.RescheduleTicketReceiptName = request.RescheduleTicketReceiptName;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entityExist);
                    }
                }
            }

            //==================  Approval Document Upload

            if (!string.IsNullOrWhiteSpace(request.RescheduleApprovalDocumentsReceiptName) && !string.IsNullOrWhiteSpace(request.RescheduleApprovalDocumentData))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.RescheduleApprovalDocumentsReceiptName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.RescheduleApprovalDocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entityExist.RescheduleApprovalDocumentsReceiptPath = path;
                        entityExist.RescheduleApprovalDocumentsReceiptName = request.RescheduleApprovalDocumentsReceiptName;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entityExist);
                    }
                }
            }

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
