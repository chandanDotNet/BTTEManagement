using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Dto;
using BTTEM.Repository;
using Microsoft.EntityFrameworkCore;
using BTTEM.Data;

namespace BTTEM.MediatR.Handler
{
    public class AddMultiLevelApprovalCommandHandler : IRequestHandler<AddMultiLevelApprovalCommand, ServiceResponse<MultiLevelApprovalDto>>
    {
        private readonly IMultiLevelApprovalRepository _multiLevelApprovalRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddMultiLevelApprovalCommandHandler> _logger;
        public AddMultiLevelApprovalCommandHandler(
           IMultiLevelApprovalRepository multiLevelApprovalRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddMultiLevelApprovalCommandHandler> logger
            )
        {
            _multiLevelApprovalRepository = multiLevelApprovalRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<MultiLevelApprovalDto>> Handle(AddMultiLevelApprovalCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _multiLevelApprovalRepository.FindBy(c => c.Title == request.Title).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Multi Level Approval Already Exist");
                return ServiceResponse<MultiLevelApprovalDto>.Return409("Multi Level Approval Already Exist.");
            }
            var entity = _mapper.Map<Data.MultiLevelApproval>(request);
            entity.Id = Guid.NewGuid();
            _multiLevelApprovalRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Multi Level Approval.");
                return ServiceResponse<MultiLevelApprovalDto>.Return500();
            }
            return ServiceResponse<MultiLevelApprovalDto>.ReturnResultWith200(_mapper.Map<MultiLevelApprovalDto>(entity));
        }
    }
}