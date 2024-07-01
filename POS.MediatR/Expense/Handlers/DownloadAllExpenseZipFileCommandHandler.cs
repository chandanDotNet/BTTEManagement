using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto.Expense;
using BTTEM.Data.Entities.Expense;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DownloadAllExpenseZipFileCommandHandler : IRequestHandler<DownloadAllExpenseZipFileCommand, List<string>>
    {
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DownloadAllExpenseZipFileCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DownloadAllExpenseZipFileCommandHandler(IMapper mapper,
            IMasterExpenseRepository masterExpenseRepository,
            IExpenseRepository expenseRepository,
            IExpenseDocumentRepository expenseDocumentRepository,
            ITripItineraryRepository tripItineraryRepository,
            ITripHotelBookingRepository tripHotelBookingRepository,
            IItineraryTicketBookingRepository itineraryTicketBookingRepository,
            ILogger<DownloadAllExpenseZipFileCommandHandler> logger,
           IUnitOfWork<POSDbContext> uow, IWebHostEnvironment webHostEnvironment,
           PathHelper pathHelper)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _expenseRepository = expenseRepository;
            _expenseDocumentRepository = expenseDocumentRepository;
            _tripItineraryRepository = tripItineraryRepository;
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _itineraryTicketBookingRepository = itineraryTicketBookingRepository;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _uow = uow;
            _pathHelper = pathHelper;
        }
        public async Task<List<string>> Handle(DownloadAllExpenseZipFileCommand request, CancellationToken cancellationToken)
        {
            List<string> result = new List<string>();
            List<ExpenseDocument> documents = new List<ExpenseDocument>();
            List<TripHotelBooking> hotelBooking = new List<TripHotelBooking>();
            List<ItineraryTicketBooking> ticketBooking = new List<ItineraryTicketBooking>();
            var masterExpense = await _masterExpenseRepository.FindAsync(request.MasterExpenseId);
            var expense = await _expenseRepository.All.Where(x => x.MasterExpenseId == request.MasterExpenseId).ToListAsync();
            foreach (var item in expense)
            {
                var expenseDocuments = await _expenseDocumentRepository.All.Where(x => x.ExpenseId == item.Id).ToListAsync();
                documents.AddRange(expenseDocuments);
            }
            if (masterExpense.TripId.HasValue)
            {
                var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == masterExpense.TripId).FirstOrDefaultAsync();
                hotelBooking = await _tripHotelBookingRepository.All.Where(x => x.TripId == masterExpense.TripId).ToListAsync();
                ticketBooking = await _itineraryTicketBookingRepository.All.Where(x => x.TripItineraryId == itinerary.Id).ToListAsync();
            }
            if (documents.Count > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.Attachments);

                documents.ForEach(item =>
                {
                    if (item.ReceiptPath != null)
                    {
                        item.ReceiptPath = Path.Combine(filePath, item.ReceiptPath);
                        result.Add(item.ReceiptPath);
                    }
                });
            }
            if (ticketBooking.Count > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TravelDeskAttachments);
                ticketBooking.ForEach(item =>
                {
                    if (item.TicketReceiptPath != null)
                    {
                        item.TicketReceiptPath = Path.Combine(filePath, item.TicketReceiptPath);
                        result.Add(item.TicketReceiptPath);
                    }
                    if (item.ApprovalDocumentsReceiptPath != null)
                    {
                        item.ApprovalDocumentsReceiptPath = Path.Combine(filePath, item.ApprovalDocumentsReceiptPath);
                        result.Add(item.ApprovalDocumentsReceiptPath);
                    }
                });
            }
            if (hotelBooking.Count > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TravelDeskAttachments);

                hotelBooking.ForEach(item =>
                {
                    if (item.BillReceiptPath != null)
                    {
                        item.BillReceiptPath = Path.Combine(filePath, item.BillReceiptPath);
                        result.Add(item.BillReceiptPath);
                    }
                    if (item.ApprovalDocumentsReceiptPath != null)
                    {
                        item.ApprovalDocumentsReceiptPath = Path.Combine(filePath, item.ApprovalDocumentsReceiptPath);
                        result.Add(item.ApprovalDocumentsReceiptPath);
                    }
                });
            }

            return result;
        }
    }
}
