using AutoMapper;
using BTTEM.MediatR.CompanyProfile.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, ServiceResponse<bool>>
    {

        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly ICompanyAccountRepository _companyAccountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteCompanyCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public DeleteCompanyCommandHandler(
            ICompanyProfileRepository companyProfileRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCompanyCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            ICompanyAccountRepository companyAccountRepository)
        {
            _companyProfileRepository = companyProfileRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _companyAccountRepository = companyAccountRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _companyAccountRepository.FindAsync(request.Id); 
            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            } 
            _companyAccountRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
