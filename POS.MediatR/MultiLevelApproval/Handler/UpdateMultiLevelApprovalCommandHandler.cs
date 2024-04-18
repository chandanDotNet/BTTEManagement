using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handler
{
    public class UpdateMultiLevelApprovalCommandHandler : IRequestHandler<UpdateMultiLevelApprovalCommand, ServiceResponse<bool>>
    {
        private readonly IMultiLevelApprovalRepository _multiLevelApprovalRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateMultiLevelApprovalCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateMultiLevelApprovalCommandHandler(
            IMultiLevelApprovalRepository multiLevelApprovalRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateMultiLevelApprovalCommandHandler> logger,
            IMapper mapper
            )
        {
            _multiLevelApprovalRepository = multiLevelApprovalRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateMultiLevelApprovalCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _multiLevelApprovalRepository.FindBy(c => c.Title == request.Title && c.Id != request.Id).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Multi Level Approval already Exists.");
                return ServiceResponse<bool>.Return409("Multi Level Approval already Exists.");
            }
            existingEntity = await _multiLevelApprovalRepository.FindAsync(request.Id);

            if (existingEntity == null)
            {
                _logger.LogError("Multi Level Approval does not Exists.");
                return ServiceResponse<bool>.Return409("Multi Level Approval does not Exists.");
            }
            existingEntity.Title = request.Title;
            existingEntity.IsActive = request.IsActive;
            _multiLevelApprovalRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Multi Level Approval");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}