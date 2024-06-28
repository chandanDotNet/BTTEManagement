using AutoMapper;
using BTTEM.Data.Dto.Expense;
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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DownloadAllExpenseZipFileCommandHandler : IRequestHandler<DownloadAllExpenseZipFileCommand, bool>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DownloadAllExpenseZipFileCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DownloadAllExpenseZipFileCommandHandler(IMapper mapper,
            IExpenseRepository expenseRepository,
            ITripItineraryRepository tripItineraryRepository,
            ITripHotelBookingRepository tripHotelBookingRepository,
            IItineraryTicketBookingRepository itineraryTicketBookingRepository,
            ILogger<DownloadAllExpenseZipFileCommandHandler> logger,
           IUnitOfWork<POSDbContext> uow, IWebHostEnvironment webHostEnvironment,
           PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _tripItineraryRepository = tripItineraryRepository;
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _itineraryTicketBookingRepository = itineraryTicketBookingRepository;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _uow = uow;
            _pathHelper = pathHelper;
        }
        public async Task<bool> Handle(DownloadAllExpenseZipFileCommand request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepository.All.Where(x => x.MasterExpenseId == request.MasterExpenseId).ToListAsync();
            var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == expense[0].TripId).FirstOrDefaultAsync();
            var hotelBooking = await _tripHotelBookingRepository.All.Where(x => x.TripId == expense[0].TripId).ToListAsync();
            var ticketBooking = await _itineraryTicketBookingRepository.All.Where(x => x.TripItineraryId == itinerary.Id).ToListAsync();



            return true;
        }
    }
}
