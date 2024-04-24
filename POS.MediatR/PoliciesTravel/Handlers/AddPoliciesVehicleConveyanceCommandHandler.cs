using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
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
    public class AddPoliciesVehicleConveyanceCommandHandler : IRequestHandler<AddPoliciesVehicleConveyanceCommand, ServiceResponse<PoliciesVehicleConveyanceDto>>
    {
        private readonly IPoliciesVehicleConveyanceRepository _policiesVehicleConveyanceRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddPoliciesVehicleConveyanceCommandHandler> _logger;
        public AddPoliciesVehicleConveyanceCommandHandler(
           IPoliciesVehicleConveyanceRepository policiesVehicleConveyanceRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddPoliciesVehicleConveyanceCommandHandler> logger
            )
        {
            _policiesVehicleConveyanceRepository = policiesVehicleConveyanceRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<PoliciesVehicleConveyanceDto>> Handle(AddPoliciesVehicleConveyanceCommand request, CancellationToken cancellationToken)
        {
            //var entityExist = await _travelsModeRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            //if (entityExist != null)
            //{
            //    _logger.LogError("Policies Name already exist.");
            //    return ServiceResponse<TravelModeDto>.Return409("Policies Name already exist.");
            //}
            foreach (var tv in request.VehicleConveyanceData)
            {
                var entity = _mapper.Map<Data.PoliciesVehicleConveyance>(tv);
                entity.Id = Guid.NewGuid();
                entity.IsDeleted =false;

                _policiesVehicleConveyanceRepository.Add(entity);
            }

            //var entity = _mapper.Map<Data.TravelMode>(request);
            //entity.Id = Guid.NewGuid();

            //_travelsModeRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PoliciesVehicleConveyanceDto>.Return500();
            }

            //var entities = await _conveyanceRepository.AllIncluding(c => c.conveyancesItem).ToListAsync();
            //var entityDto = _mapper.Map<List<ConveyanceDto>>(entities);
            //var entityDto = _mapper.Map<TravelModeDto>(entity);
            //return ServiceResponse<TravelModeDto>.ReturnResultWith200(entityDto);
            return ServiceResponse<PoliciesVehicleConveyanceDto>.ReturnSuccess();
        }

    }
}
