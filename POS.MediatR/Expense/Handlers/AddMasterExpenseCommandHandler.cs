using AutoMapper;
using BTTEM.Data;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddMasterExpenseCommandHandler : IRequestHandler<AddMasterExpenseCommand, ServiceResponse<MasterExpenseDto>>
    {

        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddMasterExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddMasterExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddMasterExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }


        public async Task<ServiceResponse<MasterExpenseDto>> Handle(AddMasterExpenseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<MasterExpense>(request);
            entity.Id= Guid.NewGuid();

            _masterExpenseRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<MasterExpenseDto>.Return500();
            }

            var industrydto = _mapper.Map<MasterExpenseDto>(entity);
            return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(industrydto);
        }
    }
}
