using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Handlers;
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
    public class UpdatePoliciesLodgingFoodingCommandHandler : IRequestHandler<UpdatePoliciesLodgingFoodingCommand, ServiceResponse<bool>>
    {

        private readonly IPoliciesLodgingFoodingRepository _policiesLodgingFoodingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdatePoliciesLodgingFoodingCommandHandler> _logger;
        public UpdatePoliciesLodgingFoodingCommandHandler(
           IPoliciesLodgingFoodingRepository policiesLodgingFoodingRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdatePoliciesLodgingFoodingCommandHandler> logger
            )
        {
            _policiesLodgingFoodingRepository = policiesLodgingFoodingRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdatePoliciesLodgingFoodingCommand request, CancellationToken cancellationToken)
        {
            var lodgingFoodingExitUpdate = _mapper.Map<Data.PoliciesLodgingFooding>(request);

            var LodgingFoodingExit = await _policiesLodgingFoodingRepository.All.Where(c=>c.PoliciesDetailId==request.PoliciesDetailId).FirstOrDefaultAsync();

            LodgingFoodingExit.IsMetroCities = lodgingFoodingExitUpdate.IsMetroCities;
            LodgingFoodingExit.OtherCities = lodgingFoodingExitUpdate.OtherCities;
            LodgingFoodingExit.MetroCitiesUptoAmount = lodgingFoodingExitUpdate.MetroCitiesUptoAmount;
            LodgingFoodingExit.OtherCitiesUptoAmount = lodgingFoodingExitUpdate.OtherCitiesUptoAmount;
            LodgingFoodingExit.BudgetAmount = lodgingFoodingExitUpdate.BudgetAmount;
            LodgingFoodingExit.IsBudget = lodgingFoodingExitUpdate.IsBudget;
            LodgingFoodingExit.IsFoodActuals = lodgingFoodingExitUpdate.IsFoodActuals;
            LodgingFoodingExit.PoliciesDetailId = lodgingFoodingExitUpdate.PoliciesDetailId;
            LodgingFoodingExit.IsBillRequired = lodgingFoodingExitUpdate.IsBillRequired;
            LodgingFoodingExit.DeductionPercentage = lodgingFoodingExitUpdate.DeductionPercentage;

            _policiesLodgingFoodingRepository.Update(LodgingFoodingExit);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Lodging Fooding Mode.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith201(true);
        }
    }
}
