using AutoMapper;
using BTTEM.MediatR.CompanyProfile.Commands;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    internal class UpdateGSTCommandHandler : IRequestHandler<UpdateGSTCommand, ServiceResponse<CompanyProfileDto>>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateGSTCommand> _logger;

        public UpdateGSTCommandHandler(
            ICompanyProfileRepository companyProfileRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateGSTCommand> logger
           )
        {
            _companyProfileRepository = companyProfileRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<CompanyProfileDto>> Handle(UpdateGSTCommand request, CancellationToken cancellationToken)
        {
            var companyProfile = await _companyProfileRepository.All.FirstOrDefaultAsync();
            if (companyProfile != null)
            {
                 companyProfile.GST = request.GST;
                _companyProfileRepository.Update(companyProfile);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Company Profile.");
                return ServiceResponse<CompanyProfileDto>.Return500();
            }            
            var result = _mapper.Map<CompanyProfileDto>(companyProfile);           
            return ServiceResponse<CompanyProfileDto>.ReturnResultWith200(result);
        }
    }
}