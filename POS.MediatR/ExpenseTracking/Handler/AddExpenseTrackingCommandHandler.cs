using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.TripTracking.Handler;
using BTTEM.Repository;
using MediatR;
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

namespace BTTEM.MediatR.ExpenseTracking.Handler
{
    public class AddExpenseTrackingCommandHandler : IRequestHandler<AddExpenseTrackingCommand, ServiceResponse<ExpenseTrackingDto>>
    {
        private readonly IExpenseTrackingRepository _expenseTrackingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddExpenseTrackingCommandHandler> _logger;

        public AddExpenseTrackingCommandHandler(
           IExpenseTrackingRepository expenseTrackingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddExpenseTrackingCommandHandler> logger
          )
        {
            _expenseTrackingRepository = expenseTrackingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<ExpenseTrackingDto>> Handle(AddExpenseTrackingCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.ExpenseTracking>(request);
            var id = Guid.NewGuid();
            entity.Id = id;
            _expenseTrackingRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip");
                return ServiceResponse<ExpenseTrackingDto>.Return500();
            }
            var tripTracking = _mapper.Map<ExpenseTrackingDto>(entity);
            return ServiceResponse<ExpenseTrackingDto>.ReturnResultWith200(tripTracking);
        }
    }
}