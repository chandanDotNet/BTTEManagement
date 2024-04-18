using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
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
    public class GetMultiLevelApprovalQueryHandler : IRequestHandler<GetMultiLevelApprovalQuery, ServiceResponse<MultiLevelApprovalDto>>
    {
        private readonly IMultiLevelApprovalRepository _multiLevelApprovalRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMultiLevelApprovalQueryHandler> _logger;

        public GetMultiLevelApprovalQueryHandler(
            IMultiLevelApprovalRepository multiLevelApprovalRepository,
            IMapper mapper,
            ILogger<GetMultiLevelApprovalQueryHandler> logger)
        {
            _multiLevelApprovalRepository = multiLevelApprovalRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<MultiLevelApprovalDto>> Handle(GetMultiLevelApprovalQuery request, CancellationToken cancellationToken)
        {
            var entity = await _multiLevelApprovalRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<MultiLevelApprovalDto>.ReturnResultWith200(_mapper.Map<MultiLevelApprovalDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<MultiLevelApprovalDto>.Return404();
            }
        }
    }
}