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
using BTTEM.Data;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Handler
{
    public class AddApprovalLevelTypeCommandHandler : IRequestHandler<AddApprovalLevelTypeCommand, ServiceResponse<ApprovalLevelTypeDto>>
    {
        private readonly IApprovalLevelTypeRepository _approvalLevelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddApprovalLevelTypeCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public AddApprovalLevelTypeCommandHandler(IMapper mapper, ILogger<AddApprovalLevelTypeCommandHandler> logger, 
            IWebHostEnvironment webHostEnvironment, PathHelper pathHelper,
            IUnitOfWork<POSDbContext> uow, IApprovalLevelTypeRepository approvalLevelRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _uow = uow;
            _approvalLevelRepository= approvalLevelRepository;
        }

        public async Task<ServiceResponse<ApprovalLevelTypeDto>> Handle(AddApprovalLevelTypeCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _approvalLevelRepository
                            .All.FirstOrDefaultAsync(c => c.TypeName == request.TypeName);
            if (existingEntity != null)
            {
                _logger.LogError("Approval Type Already Exist.");
                return ServiceResponse<ApprovalLevelTypeDto>.Return409("Approval Type  Already Exist.");
            }
            var entity = _mapper.Map<Data.ApprovalLevelType>(request);
            entity.Id = Guid.NewGuid();
            _approvalLevelRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<ApprovalLevelTypeDto>.Return500();
            }
            return ServiceResponse<ApprovalLevelTypeDto>.ReturnResultWith200(_mapper.Map<ApprovalLevelTypeDto>(entity));
        }
    }
}
