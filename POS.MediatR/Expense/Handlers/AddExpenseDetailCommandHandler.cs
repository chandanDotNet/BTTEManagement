using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Expense.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class AddExpenseDetailCommandHandler : IRequestHandler<AddExpenseDetailCommand, ServiceResponse<ExpenseDetailDto>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IExpenseDetailRepository _expenseDetailRepository;
        private readonly ILogger<AddExpenseDetailCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        public AddExpenseDetailCommandHandler(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository, IExpenseDetailRepository expenseDetailRepository,
            ILogger<AddExpenseDetailCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _expenseDetailRepository = expenseDetailRepository;
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ExpenseDetailDto>> Handle(AddExpenseDetailCommand request, CancellationToken cancellationToken)
        {

            var entity = _mapper.Map<ExpenseDetail>(request);
            entity.Id = Guid.NewGuid();
            _expenseDetailRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense");
                return ServiceResponse<ExpenseDetailDto>.Return500();
            }

            var industrydto = _mapper.Map<ExpenseDetailDto>(entity);
            
            return ServiceResponse<ExpenseDetailDto>.ReturnResultWith200(industrydto);
        }
        }
}
