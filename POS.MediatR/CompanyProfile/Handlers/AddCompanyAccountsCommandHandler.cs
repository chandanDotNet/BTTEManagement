using AutoMapper;
using BTTEM.MediatR.CompanyProfile.Commands;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    public class AddCompanyAccountsCommandHandler : IRequestHandler<AddCompanyAccountsCommand, ServiceResponse<bool>>
    {
        private readonly ICompanyAccountRepository _companyAccountRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCompanyAccountsCommandHandler> _logger;
        public AddCompanyAccountsCommandHandler(ICompanyAccountRepository companyAccountRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddCompanyAccountsCommandHandler> logger)
        {
            _companyAccountRepository = companyAccountRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(AddCompanyAccountsCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.CompanyAccountsList)
            {
                var entityExist = _companyAccountRepository.All.Where(x => x.AccountName == item.AccountName).FirstOrDefault();
                if (entityExist == null)
                {
                    var entity = _mapper.Map<BTTEM.Data.CompanyAccount>(item);
                    entity.Id = Guid.NewGuid();
                    entity.CompanyProfileId = new Guid("7B29AF56-6529-4999-B5C6-08DBCBEA0CB2");
                    _companyAccountRepository.Add(entity);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        _logger.LogError("Error while saving Grade");
                        return ServiceResponse<bool>.Return500();
                    }
                }
            }
            var dtoEntity = _mapper.Map<bool>(true);
            return ServiceResponse<bool>.ReturnResultWith200(dtoEntity);
        }
    }
}
