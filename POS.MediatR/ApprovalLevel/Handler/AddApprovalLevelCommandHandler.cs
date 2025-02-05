using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.ApprovalLevel.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
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
    public class AddApprovalLevelCommandHandler : IRequestHandler<AddApprovalLevelCommand, ServiceResponse<ApprovalLevelDto>>
    {
        private readonly IApprovalLevelRepository _approvalLevelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddApprovalLevelCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public AddApprovalLevelCommandHandler(IMapper mapper, ILogger<AddApprovalLevelCommandHandler> logger,
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

        public async Task<ServiceResponse<ApprovalLevelDto>> Handle(AddApprovalLevelCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _approvalLevelRepository
                            .All.FirstOrDefaultAsync(c => c.LevelName == request.LevelName);
            if (existingEntity != null)
            {
                _logger.LogError("Approval Already Exist.");
                return ServiceResponse<ApprovalLevelDto>.Return409("Approval  Already Exist.");
            }
            var entity = _mapper.Map<Data.ApprovalLevel>(request);
            //entity.Id = Guid.NewGuid();
            //entity.ApprovalLevelUsers.ForEach(item =>
            //{
              
            //});
            _approvalLevelRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<ApprovalLevelDto>.Return500();
            }
            return ServiceResponse<ApprovalLevelDto>.ReturnResultWith200(_mapper.Map<ApprovalLevelDto>(entity));
        }
    }
}