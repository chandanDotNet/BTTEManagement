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
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class DonwloadTravelDocumentCommandHandler : IRequestHandler<DonwloadTravelDocumentCommand, string>
    {

        private readonly ITravelDocumentRepository _travelDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DonwloadTravelDocumentCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DonwloadTravelDocumentCommandHandler(
            ITravelDocumentRepository travelDocumentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<DonwloadTravelDocumentCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDocumentRepository = travelDocumentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<string> Handle(DonwloadTravelDocumentCommand request, CancellationToken cancellationToken)
        {
            var expense = await _travelDocumentRepository.FindAsync(request.Id);
            if (expense == null)
            {
                return "";
            }
            string contentRootPath = _webHostEnvironment.WebRootPath;
            return Path.Combine(contentRootPath, _pathHelper.TravelDocument, expense.ReceiptPath);
        }
    }
}
