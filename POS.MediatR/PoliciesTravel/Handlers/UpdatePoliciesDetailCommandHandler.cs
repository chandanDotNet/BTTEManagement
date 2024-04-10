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

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class UpdatePoliciesDetailCommandHandler : IRequestHandler<UpdatePoliciesDetailCommand, ServiceResponse<bool>>
    {

        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdatePoliciesDetailCommandHandler> _logger;
        public UpdatePoliciesDetailCommandHandler(
           IPoliciesDetailRepository policiesDetailRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdatePoliciesDetailCommandHandler> logger
            )
        {
            _policiesDetailRepository = policiesDetailRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdatePoliciesDetailCommand request, CancellationToken cancellationToken)
        {
            var policiesDetailUpdate = _mapper.Map<Data.PoliciesDetail>(request);

            var policiesDetailExit = await _policiesDetailRepository.FindAsync(request.Id);

            policiesDetailExit.Name = policiesDetailUpdate.Name;
            policiesDetailExit.Description = policiesDetailUpdate.Description;
            policiesDetailExit.GradeId = policiesDetailUpdate.GradeId;
            policiesDetailExit.Document = policiesDetailUpdate.Document;
            policiesDetailExit.DailyAllowance = policiesDetailUpdate.DailyAllowance;
            policiesDetailExit.IsActive= policiesDetailUpdate.IsActive;


            _policiesDetailRepository.Update(policiesDetailExit);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Lodging Fooding Mode.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith201(true);


        }
    }
}
