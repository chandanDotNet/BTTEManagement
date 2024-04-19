using BTTEM.MediatR.Command;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

namespace BTTEM.MediatR.Handler
{
    public class DeleteVehicleManagementCommandHandler : IRequestHandler<DeleteVehicleManagementCommand, ServiceResponse<bool>>
    {
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteVehicleManagementCommandHandler> _logger;
        public DeleteVehicleManagementCommandHandler(
            IVehicleManagementRepository vehicleManagementRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteVehicleManagementCommandHandler> logger
            )
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteVehicleManagementCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _vehicleManagementRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                _logger.LogError("Vehicle Management does not Exist");
                return ServiceResponse<bool>.Return409("Vehicle Management does not  Exist.");
            }
            //var exitingExpense = _multiLevelApprovalRepository.AllIncluding(c => c.ExpenseCategory).Any(c => c.ExpenseCategory.Id == existingEntity.Id);
            //if (exitingExpense)
            //{
            //    _logger.LogError("Expense Category can not be Deleted because it is use in Expense");
            //    return ServiceResponse<bool>.Return409("Expense Category can not be Deleted because it is use in Expense.");
            //}

            _vehicleManagementRepository.Delete(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense Category.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}