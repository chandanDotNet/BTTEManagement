using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handler
{
    public class GetAllVehicleManagementQueryHandler : IRequestHandler<GetAllVehicleManagementQuery, List<VehicleManagementDto>>
    {
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private readonly IMapper _mapper;

        public GetAllVehicleManagementQueryHandler(
            IVehicleManagementRepository vehicleManagementRepository,
            IMapper mapper)
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _mapper = mapper;
        }

        public async Task<List<VehicleManagementDto>> Handle(GetAllVehicleManagementQuery request, CancellationToken cancellationToken)
        {
            var entities = await _vehicleManagementRepository.All.ToListAsync();
            var dtoEntities = _mapper.Map<List<VehicleManagementDto>>(entities);
            return dtoEntities;
        }
    }
}