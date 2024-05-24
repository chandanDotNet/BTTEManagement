using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
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

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DonwloadTravelDeskCommandHandler : IRequestHandler<DonwloadTravelDeskCommand, string>
    {

        private readonly ITravelDeskExpenseRepository _travelDeskExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DonwloadTravelDeskCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DonwloadTravelDeskCommandHandler(
            ITravelDeskExpenseRepository travelDeskExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<DonwloadTravelDeskCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDeskExpenseRepository = travelDeskExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<string> Handle(DonwloadTravelDeskCommand request, CancellationToken cancellationToken)
        {
            var expense = await _travelDeskExpenseRepository.FindAsync(request.Id);
            if (expense == null)
            {
                return "";
            }
            string contentRootPath = _webHostEnvironment.WebRootPath;
            return Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments, expense.ReceiptPath);
        }
    }
}
