using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
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

namespace BTTEM.MediatR.Expense.Handlers
{
    public class UpdateExpenseStatusCommandHandler : IRequestHandler<UpdateExpenseStatusCommand, ServiceResponse<bool>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseStatusCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateExpenseStatusCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateExpenseStatusCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _expenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            entityExist.Status = request.Status;

            _expenseRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
