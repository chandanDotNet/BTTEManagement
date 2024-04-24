using AutoMapper;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class UpdatePoliciesVehicleConveyanceCommandHandler : IRequestHandler<UpdatePoliciesVehicleConveyanceCommand, ServiceResponse<bool>>
    {

        private readonly IPoliciesVehicleConveyanceRepository _policiesVehicleConveyanceRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdatePoliciesVehicleConveyanceCommandHandler> _logger;
        public UpdatePoliciesVehicleConveyanceCommandHandler(
           IPoliciesVehicleConveyanceRepository policiesVehicleConveyanceRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdatePoliciesVehicleConveyanceCommandHandler> logger
            )
        {
            _policiesVehicleConveyanceRepository = policiesVehicleConveyanceRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdatePoliciesVehicleConveyanceCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.VehicleConveyanceData)
            {
                var travelsModeUpdate = _mapper.Map<Data.PoliciesVehicleConveyance>(tv);

                var travelsModeExit = await _policiesVehicleConveyanceRepository.All.Where(a => a.Id == tv.Id).FirstOrDefaultAsync();

                travelsModeExit.RatePerKM = travelsModeUpdate.RatePerKM;
                travelsModeExit.MaintenanceCharges = travelsModeUpdate.MaintenanceCharges;
                travelsModeExit.PoliciesDetailId = travelsModeUpdate.PoliciesDetailId;
                travelsModeExit.VehicleId = travelsModeUpdate.VehicleId;
                travelsModeExit.Id = travelsModeUpdate.Id;
                travelsModeExit.VehicleName = travelsModeUpdate.VehicleName;
                
                _policiesVehicleConveyanceRepository.Update(travelsModeExit);

            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Policies Vehicle Conveyance.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith201(true);
        }

    }
}
