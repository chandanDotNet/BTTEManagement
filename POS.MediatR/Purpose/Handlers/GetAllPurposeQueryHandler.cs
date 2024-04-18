using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Purpose.Handlers
{
    public class GetAllPurposeQueryHandler : IRequestHandler<GetAllPurposeQuery, List<PurposeDto>>
    {
        private readonly IPurposeRepository _purposeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPurposeQueryHandler> _logger;

        public GetAllPurposeQueryHandler(
           IPurposeRepository purposeRepository,
            IMapper mapper,
            ILogger<GetAllPurposeQueryHandler> logger)
        {
            _purposeRepository = purposeRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<List<PurposeDto>> Handle(GetAllPurposeQuery request, CancellationToken cancellationToken)
        {
            var entities = await _purposeRepository.All.Where(a=>a.IsDeleted==false).ToListAsync();
            return _mapper.Map<List<PurposeDto>>(entities);
        }

    }
}
