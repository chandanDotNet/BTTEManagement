using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handler
{
    public class GetAllMultiLevelApprovalQueryHandler : IRequestHandler<GetAllMultiLevelApprovalQuery, List<MultiLevelApprovalDto>>
    {
        private readonly IMultiLevelApprovalRepository _multiLevelApprovalRepository;
        private readonly IMapper _mapper;

        public GetAllMultiLevelApprovalQueryHandler(
            IMultiLevelApprovalRepository multiLevelApprovalRepository,
            IMapper mapper)
        {
            _multiLevelApprovalRepository = multiLevelApprovalRepository;
            _mapper = mapper;
        }

        public async Task<List<MultiLevelApprovalDto>> Handle(GetAllMultiLevelApprovalQuery request, CancellationToken cancellationToken)
        {
            var entities = await _multiLevelApprovalRepository.All.ToListAsync();
            var dtoEntities = _mapper.Map<List<MultiLevelApprovalDto>>(entities);
            return dtoEntities;
        }
    }
}