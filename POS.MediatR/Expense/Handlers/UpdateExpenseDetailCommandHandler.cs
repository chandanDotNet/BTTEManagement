using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class UpdateExpenseDetailCommandHandler : IRequestHandler<UpdateExpenseDetailCommand, ServiceResponse<bool>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IExpenseDetailRepository _expenseDetailRepository;
        private readonly ILogger<UpdateExpenseDetailCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        public UpdateExpenseDetailCommandHandler(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository, IExpenseDetailRepository expenseDetailRepository,
            ILogger<UpdateExpenseDetailCommandHandler> logger,
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

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseDetailCommand request, CancellationToken cancellationToken)
        {
            //if (request.Id.HasValue)
            //{
            //    var entityExist = await _expenseDetailRepository.FindAsync(request.Id.Value);
            //    if (entityExist == null)
            //    {
            //        _logger.LogError("Expense does not exists.");
            //        return ServiceResponse<bool>.Return409("Expense does not exists.");
            //    }

            //    _expenseDetailRepository.Update(entityExist);
            //}
            //else
            //{
            //    var entity = _mapper.Map<ExpenseDetail>(request);
            //    entity.Id = Guid.NewGuid();
            //    _expenseDetailRepository.Add(entity);
            //}

            if (request.CostCenterCheck == false)
            {
                var entity = _mapper.Map<ExpenseDetail>(request);
                entity.Id = Guid.NewGuid();
                _expenseDetailRepository.Add(entity);

                if (request.IsTaxable == true)
                {
                    var expense = await _expenseRepository.FindAsync(request.ExpenseId.Value);
                    expense.BillType = "GST";
                    _expenseRepository.Update(expense);
                }
            }
            else
            {
                var expenseDetails =  await _expenseDetailRepository.FindAsync(request.Id.Value);
                expenseDetails.CostCenter = request.CostCenter;
                _expenseDetailRepository.Update(expenseDetails);
            }

            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
