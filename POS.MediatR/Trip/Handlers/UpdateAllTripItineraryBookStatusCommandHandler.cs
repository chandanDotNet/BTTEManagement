using AutoMapper;
using BTTEM.MediatR.Handlers;
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
    public class UpdateAllTripItineraryBookStatusCommandHandler : IRequestHandler<UpdateAllTripItineraryBookStatusCommand, ServiceResponse<bool>>
    {
        private readonly ITripItineraryRepository _allTripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateAllTripItineraryBookStatusCommandHandler> _logger;
        private readonly ITripHotelBookingRepository _allTripHotelBookingRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateAllTripItineraryBookStatusCommandHandler(
           ITripItineraryRepository allTripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateAllTripItineraryBookStatusCommandHandler> logger,
            ITripHotelBookingRepository allTripHotelBookingRepository,
             IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
          )
        {
            _allTripItineraryRepository = allTripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _allTripHotelBookingRepository = allTripHotelBookingRepository;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateAllTripItineraryBookStatusCommand request, CancellationToken cancellationToken)
        {
            request.AllTripItineraryBookStatusList.ForEach(item =>
            {
                if (item.IsItinerary == true)
                {
                    var entityExist = _allTripItineraryRepository.FindBy(v => v.Id == item.Id).FirstOrDefaultAsync().Result;
                    if (!string.IsNullOrWhiteSpace(item.BookStatus))
                    {
                        entityExist.BookStatus = item.BookStatus;
                    }
                    if (!string.IsNullOrWhiteSpace(item.ApprovalStatus))
                    {
                        entityExist.ApprovalStatus = item.ApprovalStatus;
                    }
                    if (item.ApprovalStatusDate.HasValue)
                    {
                        entityExist.ApprovalStatusDate = item.ApprovalStatusDate;
                    }
                    if (item.ApprovalStatusBy.HasValue)
                    {
                        entityExist.ApprovalStatusBy = item.ApprovalStatusBy;
                    }
                    if (item.Amount > 0)
                    {
                        entityExist.Amount = item.Amount;
                    }

                    //==================  Ticket Upload

                    if (!string.IsNullOrWhiteSpace(item.TicketReceiptName) && !string.IsNullOrWhiteSpace(item.TicketDocumentData))
                    {
                        string contentRootPath = _webHostEnvironment.WebRootPath;
                        var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        var extension = Path.GetExtension(item.TicketReceiptName);
                        var id = Guid.NewGuid();
                        var path = $"{id}.{extension}";
                        var documentPath = Path.Combine(pathToSave, path);
                        string base64 = item.TicketDocumentData.Split(',').LastOrDefault();
                        if (!string.IsNullOrWhiteSpace(base64))
                        {
                            byte[] bytes = Convert.FromBase64String(base64);
                            try
                            {
                                File.WriteAllBytesAsync($"{documentPath}", bytes);
                                entityExist.TicketReceiptPath = path;
                            }
                            catch
                            {
                                _logger.LogError("Error while saving files", entityExist);
                            }
                        }
                    }

                    //==================  Approval Document Upload

                    if (!string.IsNullOrWhiteSpace(item.ApprovalDocumentsReceiptName) && !string.IsNullOrWhiteSpace(item.ApprovalDocumentData))
                    {
                        string contentRootPath = _webHostEnvironment.WebRootPath;
                        var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        var extension = Path.GetExtension(item.ApprovalDocumentsReceiptName);
                        var id = Guid.NewGuid();
                        var path = $"{id}.{extension}";
                        var documentPath = Path.Combine(pathToSave, path);
                        string base64 = item.ApprovalDocumentData.Split(',').LastOrDefault();
                        if (!string.IsNullOrWhiteSpace(base64))
                        {
                            byte[] bytes = Convert.FromBase64String(base64);
                            try
                            {
                                File.WriteAllBytesAsync($"{documentPath}", bytes);
                                entityExist.ApprovalDocumentsReceiptPath = path;
                            }
                            catch
                            {
                                _logger.LogError("Error while saving files", entityExist);
                            }
                        }
                    }

                    //=================

                    if (!item.ExpenseId.HasValue || item.ExpenseId.Value == Guid.Empty)
                    {
                        //not valid GUID
                    }
                    else
                    {
                        entityExist.ExpenseId = item.ExpenseId;
                    }

                    _allTripItineraryRepository.Update(entityExist);
                }
                else
                {
                    var entityExist = _allTripHotelBookingRepository.FindBy(v => v.Id == item.Id).FirstOrDefaultAsync().Result;
                    if (!string.IsNullOrWhiteSpace(item.BookStatus))
                    {
                        entityExist.BookStatus = item.BookStatus;
                    }
                    if (!string.IsNullOrWhiteSpace(item.ApprovalStatus))
                    {
                        entityExist.ApprovalStatus = item.ApprovalStatus;
                    }
                    if (item.ApprovalStatusDate.HasValue)
                    {
                        entityExist.ApprovalStatusDate = item.ApprovalStatusDate;
                    }
                    if (item.ApprovalStatusBy.HasValue)
                    {
                        entityExist.ApprovalStatusBy = item.ApprovalStatusBy;
                    }
                    if (item.Amount > 0)
                    {
                        entityExist.Amount = item.Amount;
                    }
                    if (!string.IsNullOrWhiteSpace(item.BookingNumber))
                    {
                        entityExist.BookingNumber = item.BookingNumber;
                    }

                    //==================  Ticket Upload

                    if (!string.IsNullOrWhiteSpace(item.TicketReceiptName) && !string.IsNullOrWhiteSpace(item.TicketDocumentData))
                    {
                        string contentRootPath = _webHostEnvironment.WebRootPath;
                        var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        var extension = Path.GetExtension(item.TicketReceiptName);
                        var id = Guid.NewGuid();
                        var path = $"{id}.{extension}";
                        var documentPath = Path.Combine(pathToSave, path);
                        string base64 = item.TicketDocumentData.Split(',').LastOrDefault();
                        if (!string.IsNullOrWhiteSpace(base64))
                        {
                            byte[] bytes = Convert.FromBase64String(base64);
                            try
                            {
                                File.WriteAllBytesAsync($"{documentPath}", bytes);
                                entityExist.BillReceiptPath = path;
                                entityExist.BillReceiptName = item.TicketReceiptName;
                            }
                            catch
                            {
                                _logger.LogError("Error while saving files", entityExist);
                            }
                        }
                    }

                    //==================  Approval Document Upload

                    if (!string.IsNullOrWhiteSpace(item.ApprovalDocumentsReceiptName) && !string.IsNullOrWhiteSpace(item.ApprovalDocumentData))
                    {
                        string contentRootPath = _webHostEnvironment.WebRootPath;
                        var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        var extension = Path.GetExtension(item.ApprovalDocumentsReceiptName);
                        var id = Guid.NewGuid();
                        var path = $"{id}.{extension}";
                        var documentPath = Path.Combine(pathToSave, path);
                        string base64 = item.ApprovalDocumentData.Split(',').LastOrDefault();
                        if (!string.IsNullOrWhiteSpace(base64))
                        {
                            byte[] bytes = Convert.FromBase64String(base64);
                            try
                            {
                                File.WriteAllBytesAsync($"{documentPath}", bytes);
                                entityExist.ApprovalDocumentsReceiptPath = path;
                                entityExist.ApprovalDocumentsReceiptName = item.ApprovalDocumentsReceiptName;
                            }
                            catch
                            {
                                _logger.LogError("Error while saving files", entityExist);
                            }
                        }
                    }

                    //=================

                    if (!item.ExpenseId.HasValue || item.ExpenseId.Value == Guid.Empty)
                    {
                        //not valid GUID
                    }
                    else
                    {
                        entityExist.ExpenseId = item.ExpenseId;
                    }
                    _allTripHotelBookingRepository.Update(entityExist);
                }
            });

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
