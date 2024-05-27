using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.RequestACall.Handler
{
    public class GetRequestCallQueryHandler : IRequestHandler<GetRequestCallQuery, List<RequestCallDto>>
    {
        private readonly IRequestCallRepository _requestCallRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRequestCallQueryHandler> _logger;

        public GetRequestCallQueryHandler(
            IRequestCallRepository requestCallRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<GetRequestCallQueryHandler> logger)
        {
            _requestCallRepository = requestCallRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RequestCallDto>> Handle(GetRequestCallQuery request, CancellationToken cancellationToken)
        {
            var requestCall = await _requestCallRepository.All.Where(a => a.AssignedTo == request.AssignedTo).ToListAsync();
            if (requestCall == null)
            {
                _logger.LogError("Request Call not found");
            }
            var requestCallDto = _mapper.Map<List<RequestCallDto>>(requestCall);
            return requestCallDto;
        }
    }
}