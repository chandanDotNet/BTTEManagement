using AutoMapper;
using BTTEM.Data.Dto.Expense;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DownloadAllExpenseZipFileCommandHandler : IRequestHandler<DownloadAllExpenseZipFileCommand, bool>
    {
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        //private readonly ITripItineraryDocumentRepository _expenseDocumentRepository;
        //private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DownloadAllExpenseZipFileCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DownloadAllExpenseZipFileCommandHandler(IMapper mapper,
            IExpenseDocumentRepository expenseDocumentRepository,
            ITripItineraryRepository tripItineraryRepository,
            ILogger<DownloadAllExpenseZipFileCommandHandler> logger,
           IUnitOfWork<POSDbContext> uow, IWebHostEnvironment webHostEnvironment,
           PathHelper pathHelper)
        {
            _expenseDocumentRepository = expenseDocumentRepository;
            _tripItineraryRepository = tripItineraryRepository;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _uow = uow;
            _pathHelper = pathHelper;
        }

        public Task<bool> Handle(DownloadAllExpenseZipFileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> Handle(DownloadAllExpenseZipFileCommand request, CancellationToken cancellationToken)
        //{

        //}
    }
}
