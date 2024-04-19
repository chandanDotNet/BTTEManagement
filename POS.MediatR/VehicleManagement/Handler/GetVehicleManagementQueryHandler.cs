using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handler
{
    public class GetVehicleManagementQueryHandler : IRequestHandler<GetVehicleManagementQuery, ServiceResponse<VehicleManagementDto>>
    {
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVehicleManagementQueryHandler> _logger;

        public GetVehicleManagementQueryHandler(
            IVehicleManagementRepository vehicleManagementRepository,
            IMapper mapper,
            ILogger<GetVehicleManagementQueryHandler> logger)
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<VehicleManagementDto>> Handle(GetVehicleManagementQuery request, CancellationToken cancellationToken)
        {
            var entity = await _vehicleManagementRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<VehicleManagementDto>.ReturnResultWith200(_mapper.Map<VehicleManagementDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<VehicleManagementDto>.Return404();
            }
        }
    }
}