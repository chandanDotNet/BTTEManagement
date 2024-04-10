using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
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

namespace BTTEM.MediatR.Handlers
{
    public class AddPoliciesLodgingFoodingCommandHandler : IRequestHandler<AddPoliciesLodgingFoodingCommand, ServiceResponse<PoliciesLodgingFoodingDto>>
    {

        private readonly IPoliciesLodgingFoodingRepository _policiesLodgingFoodingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddPoliciesLodgingFoodingCommandHandler> _logger;
        public AddPoliciesLodgingFoodingCommandHandler(
           IPoliciesLodgingFoodingRepository policiesLodgingFoodingRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddPoliciesLodgingFoodingCommandHandler> logger
            )
        {
            _policiesLodgingFoodingRepository = policiesLodgingFoodingRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }

        public async Task<ServiceResponse<PoliciesLodgingFoodingDto>> Handle(AddPoliciesLodgingFoodingCommand request, CancellationToken cancellationToken)
        {
            //var entityExist = await _policiesLodgingFoodingRepository.FindBy(c => c.BudgetAmount == request.Name).FirstOrDefaultAsync();
            //if (entityExist != null)
            //{
            //    _logger.LogError("Policies Name already exist.");
            //    return ServiceResponse<PoliciesLodgingFoodingDto>.Return409("Policies Name already exist.");
            //}

            var entity = _mapper.Map<Data.PoliciesLodgingFooding>(request);
            entity.Id = Guid.NewGuid();
            //entity.CreatedBy = Guid.Parse(_userInfoToken.Id);
            //entity.CreatedDate = DateTime.UtcNow;
            //entity.ModifiedBy = Guid.Parse(_userInfoToken.Id);

            _policiesLodgingFoodingRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PoliciesLodgingFoodingDto>.Return500();
            }
            var entityDto = _mapper.Map<PoliciesLodgingFoodingDto>(entity);
            return ServiceResponse<PoliciesLodgingFoodingDto>.ReturnResultWith200(entityDto);
        }

    }
}
