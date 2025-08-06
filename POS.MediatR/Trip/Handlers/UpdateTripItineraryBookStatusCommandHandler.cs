using AutoMapper;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

namespace BTTEM.MediatR.Handlers
{
    public class UpdateTripItineraryBookStatusCommandHandler : IRequestHandler<UpdateTripItineraryBookStatusCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripItineraryBookStatusCommandHandler> _logger;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateTripItineraryBookStatusCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripItineraryBookStatusCommandHandler> logger,
            ITripHotelBookingRepository tripHotelBookingRepository,
             IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;


        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripItineraryBookStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.IsItinerary == true)
            {
                var entityExist = await _tripItineraryRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(request.BookStatus))
                {
                    entityExist.BookStatus = request.BookStatus;
                }
                if (!string.IsNullOrWhiteSpace(request.ApprovalStatus))
                {
                    entityExist.ApprovalStatus = request.ApprovalStatus;
                }
                if (request.ApprovalStatusDate.HasValue)
                {
                    entityExist.ApprovalStatusDate = request.ApprovalStatusDate;
                }
                if (request.ApprovalStatusBy.HasValue)
                {
                    entityExist.ApprovalStatusBy = request.ApprovalStatusBy;
                }
                if (request.Amount > 0)
                {
                    entityExist.Amount = request.Amount;
                }                

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
                            entityExist.TicketReceiptPath = path;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExist);
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
                            entityExist.ApprovalDocumentsReceiptPath = path;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExist);
                        }
                    }
                }

                //=================


                if (!request.ExpenseId.HasValue || request.ExpenseId.Value == Guid.Empty)
                {
                    //not valid GUID
                }
                else
                {
                    entityExist.ExpenseId = request.ExpenseId;
                }

                _tripItineraryRepository.Update(entityExist);
            }
            else
            {
                var entityExist = await _tripHotelBookingRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(request.BookStatus))
                {
                    entityExist.BookStatus = request.BookStatus;
                }
                if (!string.IsNullOrWhiteSpace(request.ApprovalStatus))
                {
                    entityExist.ApprovalStatus = request.ApprovalStatus;
                }
                if (request.ApprovalStatusDate.HasValue)
                {
                    entityExist.ApprovalStatusDate = request.ApprovalStatusDate;
                }
                if (request.ApprovalStatusBy.HasValue)
                {
                    entityExist.ApprovalStatusBy = request.ApprovalStatusBy;
                }
                if (request.Amount>0)
                {
                    entityExist.Amount = request.Amount;
                }
                if (!string.IsNullOrWhiteSpace(request.BookingNumber))
                {
                    entityExist.BookingNumber = request.BookingNumber;
                }

                if (request.IsAmountConfirm.HasValue)
                {
                    entityExist.IsAmountConfirm = request.IsAmountConfirm;
                }

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
                            entityExist.BillReceiptPath = path;
                            entityExist.BillReceiptName = request.TicketReceiptName;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExist);
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
                            entityExist.ApprovalDocumentsReceiptPath = path;
                            entityExist.ApprovalDocumentsReceiptName = request.ApprovalDocumentsReceiptName;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExist);
                        }
                    }
                }

                //=================

                if (!request.ExpenseId.HasValue || request.ExpenseId.Value == Guid.Empty)
                {
                    //not valid GUID
                }
                else
                {
                    entityExist.ExpenseId = request.ExpenseId;
                }
                _tripHotelBookingRepository.Update(entityExist);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
