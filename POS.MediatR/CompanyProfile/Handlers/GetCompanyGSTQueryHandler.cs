using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.ContactSupport.Handler;
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

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    public class GetCompanyGSTQueryHandler : IRequestHandler<GetCompanyGSTQuery, List<CompanyGSTDto>>
    {
        private readonly ICompanyGSTRepository _companyGSTRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCompanyGSTQueryHandler> _logger;

        public GetCompanyGSTQueryHandler(
            ICompanyGSTRepository companyGSTRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<GetCompanyGSTQueryHandler> logger)
        {
            _companyGSTRepository = companyGSTRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CompanyGSTDto>> Handle(GetCompanyGSTQuery request, CancellationToken cancellationToken)
        {
            var companyGST = await _companyGSTRepository.All.Where(a => a.CompanyAccountId == request.CompanyAccountId).ToListAsync();
            if (companyGST == null)
            {
                _logger.LogError("Company GST not found");
            }
            var companyGSTDto = _mapper.Map<List<CompanyGSTDto>>(companyGST);
            return companyGSTDto;
        }
    }
}