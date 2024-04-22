using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
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
    public class GetAllVehicleManagementRateQueryHandler : IRequestHandler<GetAllVehicleManagementRateQuery, List<VehicleManagementRateDto>>
    {
        private readonly IVehicleManagementRateRepository _vehicleManagementRateRepository;
        private readonly IMapper _mapper;

        public GetAllVehicleManagementRateQueryHandler(
            IVehicleManagementRateRepository vehicleManagementRateRepository,
            IMapper mapper)
        {
            _vehicleManagementRateRepository = vehicleManagementRateRepository;
            _mapper = mapper;
        }

        public async Task<List<VehicleManagementRateDto>> Handle(GetAllVehicleManagementRateQuery request, CancellationToken cancellationToken)
        {
            var entities = await _vehicleManagementRateRepository.All
                .Where(v => v.VehicleManagementId == request.VendorManagementId)
                .Include(v => v.Grade)
                .ToListAsync();
            var dtoEntities = _mapper.Map<List<VehicleManagementRateDto>>(entities);
            return dtoEntities;
        }
    }
}