using AutoMapper;
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
    public class UpdateVehicleManagementCommandHandler : IRequestHandler<UpdateVehicleManagementCommand, ServiceResponse<bool>>
    {
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateVehicleManagementCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateVehicleManagementCommandHandler(
           IVehicleManagementRepository vehicleManagementRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateVehicleManagementCommandHandler> logger,
            IMapper mapper
            )
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateVehicleManagementCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _vehicleManagementRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Vehicle name already Exists.");
                return ServiceResponse<bool>.Return409("Vehicle name already Exists.");
            }
            existingEntity = await _vehicleManagementRepository.FindAsync(request.Id);

            if (existingEntity == null)
            {
                _logger.LogError("Vehicle name does not Exists.");
                return ServiceResponse<bool>.Return409("Vehicle name does not Exists.");
            }
            existingEntity.Name = request.Name;
            existingEntity.FuelType = request.FuelType;
            existingEntity.Description = request.Description;
            existingEntity.IsActive = request.IsActive;
            _vehicleManagementRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Vehicle name");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}