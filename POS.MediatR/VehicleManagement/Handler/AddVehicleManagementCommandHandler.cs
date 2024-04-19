using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Dto;
using BTTEM.Repository;
using Microsoft.EntityFrameworkCore;
using BTTEM.Data;

namespace BTTEM.MediatR.Handler
{
    public class AddVehicleManagementCommandHandler : IRequestHandler<AddVehicleManagementCommand, ServiceResponse<VehicleManagementDto>>
    {
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddVehicleManagementCommandHandler> _logger;
        public AddVehicleManagementCommandHandler(
           IVehicleManagementRepository vehicleManagementRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddVehicleManagementCommandHandler> logger
            )
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<VehicleManagementDto>> Handle(AddVehicleManagementCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _vehicleManagementRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Vehicle name Already Exist");
                return ServiceResponse<VehicleManagementDto>.Return409("Vehicle name Already Exist.");
            }
            var entity = _mapper.Map<VehicleManagement>(request);
            entity.Id = Guid.NewGuid();
            _vehicleManagementRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Vehicle name.");
                return ServiceResponse<VehicleManagementDto>.Return500();
            }
            return ServiceResponse<VehicleManagementDto>.ReturnResultWith200(_mapper.Map<VehicleManagementDto>(entity));
        }
    }
}