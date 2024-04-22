using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
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
using Microsoft.EntityFrameworkCore;

namespace BTTEM.MediatR.Handler
{
    public class AddVehicleManagementRateCommandHandler : IRequestHandler<AddVehicleManagementRateCommand, ServiceResponse<VehicleManagementRateDto>>
    {
        private readonly IVehicleManagementRateRepository _vehicleManagementRateRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddVehicleManagementRateCommandHandler> _logger;
        public AddVehicleManagementRateCommandHandler(
           IVehicleManagementRateRepository vehicleManagementRateRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddVehicleManagementRateCommandHandler> logger
            )
        {
            _vehicleManagementRateRepository = vehicleManagementRateRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<VehicleManagementRateDto>> Handle(AddVehicleManagementRateCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _vehicleManagementRateRepository.FindBy(c => c.VehicleManagementId == request.VehicleManagementRates.FirstOrDefault().VehicleManagementId).ToListAsync();

            var entity = _mapper.Map<List<VehicleManagementRate>>(request.VehicleManagementRates);

            if (existingEntity.Count > 0)
            {
                _vehicleManagementRateRepository.RemoveRange(existingEntity);
                entity.ForEach(item =>
                {
                    item.Id = Guid.NewGuid();
                });
                _vehicleManagementRateRepository.AddRange(entity);
            }
            else
            {
                entity.ForEach(item =>
                {
                    item.Id = Guid.NewGuid();
                });
                _vehicleManagementRateRepository.AddRange(entity);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Vehicle rate.");
                return ServiceResponse<VehicleManagementRateDto>.Return500();
            }
            return ServiceResponse<VehicleManagementRateDto>.ReturnResultWith200(_mapper.Map<VehicleManagementRateDto>(entity.FirstOrDefault()));
        }
    }
}