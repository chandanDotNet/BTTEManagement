using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.RequestACall.Handler;
using BTTEM.Repository;
using MediatR;
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

namespace BTTEM.MediatR.ContactSupport.Handler
{
    public class GetContactSupportQueryHandler : IRequestHandler<GetContactSupportQuery, List<ContactSupportDto>>
    {
        private readonly IContactSupportRepository _contactSupportRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetContactSupportQueryHandler> _logger;

        public GetContactSupportQueryHandler(
            IContactSupportRepository contactSupportRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<GetContactSupportQueryHandler> logger)
        {
            _contactSupportRepository = contactSupportRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ContactSupportDto>> Handle(GetContactSupportQuery request, CancellationToken cancellationToken)
        {
            var contactSupport = await _contactSupportRepository.All.Where(a => a.AssignedTo == request.AssignedTo).ToListAsync();
            if (contactSupport == null)
            {
                _logger.LogError("Expense not found");
            }
            var contactSupportDto = _mapper.Map<List<ContactSupportDto>>(contactSupport);
            return contactSupportDto;
        }
    }
}