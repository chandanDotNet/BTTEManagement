using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.ApprovalLevel.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Handler
{
    public class UpdateApprovalLevelCommandHandler : IRequestHandler<UpdateApprovalLevelCommand, ServiceResponse<ApprovalLevelDto>>
    {
        private readonly IApprovalLevelRepository _approvalLevelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateApprovalLevelCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public UpdateApprovalLevelCommandHandler(IMapper mapper, ILogger<UpdateApprovalLevelCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment, PathHelper pathHelper,
            IUnitOfWork<POSDbContext> uow, IApprovalLevelRepository approvalLevelRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _uow = uow;
            _approvalLevelRepository = approvalLevelRepository;
        }

        public async Task<ServiceResponse<ApprovalLevelDto>> Handle(UpdateApprovalLevelCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _approvalLevelRepository
                            .All.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (existingEntity == null)
            {
                _logger.LogError("Approval Level not Exist.");
                return ServiceResponse<ApprovalLevelDto>.Return409("Approval Level  not Exist.");
            }
            existingEntity.ApprovalLevelTypeId = request.ApprovalLevelTypeId;
            existingEntity.LevelName = request.LevelName;
            existingEntity.RoleId = request.RoleId;
            existingEntity.OrderNo = request.OrderNo;

            _approvalLevelRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<ApprovalLevelDto>.Return500();
            }
            return ServiceResponse<ApprovalLevelDto>.ReturnResultWith200(_mapper.Map<ApprovalLevelDto>(existingEntity));
        }
    }
}
