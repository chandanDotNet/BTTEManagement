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
    public class UpdatePoliciesSettingCommandHandler : IRequestHandler<UpdatePoliciesSettingCommand, ServiceResponse<bool>>
    {

        private readonly IPoliciesSettingRepository _policiesSettingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdatePoliciesSettingCommandHandler> _logger;
        public UpdatePoliciesSettingCommandHandler(
            IPoliciesSettingRepository policiesSettingRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdatePoliciesSettingCommandHandler> logger
            )
        {
            _policiesSettingRepository = policiesSettingRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdatePoliciesSettingCommand request, CancellationToken cancellationToken)
        {
            var policiesSettingUpdate = _mapper.Map<Data.PoliciesSetting>(request);

            var policiesSettingExit = await _policiesSettingRepository.All.Where(c => c.PoliciesDetailId == request.PoliciesDetailId).FirstOrDefaultAsync();

            policiesSettingExit.SubmissionDays = policiesSettingUpdate.SubmissionDays;
            policiesSettingExit.PoliciesDetailId = policiesSettingUpdate.PoliciesDetailId;
            policiesSettingExit.IsActuals = policiesSettingUpdate.IsActuals;
            policiesSettingExit.IsDeviationApprovalRequired = policiesSettingUpdate.IsDeviationApprovalRequired;
            policiesSettingExit.SetPercentage = policiesSettingUpdate.SetPercentage;
            policiesSettingExit.PercentageAmount = policiesSettingUpdate.PercentageAmount;


            _policiesSettingRepository.Update(policiesSettingExit);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Policies Setting.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith201(true);
        }
    }
}
