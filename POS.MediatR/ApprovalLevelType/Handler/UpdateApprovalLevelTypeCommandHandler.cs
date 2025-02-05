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
    public class UpdateApprovalLevelTypeCommandHandler : IRequestHandler<UpdateApprovalLevelTypeCommand, ServiceResponse<ApprovalLevelTypeDto>>
    {
        private readonly IApprovalLevelTypeRepository _approvalLevelTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateApprovalLevelTypeCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public UpdateApprovalLevelTypeCommandHandler(IMapper mapper, ILogger<UpdateApprovalLevelTypeCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment, PathHelper pathHelper,
            IUnitOfWork<POSDbContext> uow, IApprovalLevelTypeRepository approvalLevelTypeRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _uow = uow;
            _approvalLevelTypeRepository = approvalLevelTypeRepository;
        }

        public async Task<ServiceResponse<ApprovalLevelTypeDto>> Handle(UpdateApprovalLevelTypeCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _approvalLevelTypeRepository
                            .All.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (existingEntity == null)
            {
                _logger.LogError("Approval Type not Exist.");
                return ServiceResponse<ApprovalLevelTypeDto>.Return409("Approval Type  not Exist.");
            }
            existingEntity.TypeName = request.TypeName;
            existingEntity.CompanyId = request.CompanyId;

            _approvalLevelTypeRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<ApprovalLevelTypeDto>.Return500();
            }
            return ServiceResponse<ApprovalLevelTypeDto>.ReturnResultWith200(_mapper.Map<ApprovalLevelTypeDto>(existingEntity));
        }
    }
}
