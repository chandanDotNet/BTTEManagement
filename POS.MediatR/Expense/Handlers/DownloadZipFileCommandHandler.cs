using AutoMapper;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class DownloadZipFileCommandHandler : IRequestHandler<DownloadZipFileCommand, List<ExpenseDocumentDto>>
    {
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DownloadZipFileCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DownloadZipFileCommandHandler(IMapper mapper,
            IExpenseDocumentRepository expenseDocumentRepository,ILogger<DownloadZipFileCommandHandler> logger,
           IUnitOfWork<POSDbContext> uow, IWebHostEnvironment webHostEnvironment,
           PathHelper pathHelper)
        { 
            _expenseDocumentRepository= expenseDocumentRepository;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _uow= uow;
            _pathHelper = pathHelper;
        }

        public async Task<List<ExpenseDocumentDto>> Handle(DownloadZipFileCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseDocumentRepository.All.Where(e=>e.ExpenseId.Value == request.ExpenseId).ToListAsync();

            if (entityExist== null)
            {
                 _logger.LogError("Files not exists");
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.Attachments);

            entityExist.ForEach(item =>
            {
                item.ReceiptPath = Path.Combine(filePath, item.ReceiptPath);
            });

            var entityExistDto = _mapper.Map<List<ExpenseDocumentDto>>(entityExist);

            return entityExistDto;
        }
    }
}
