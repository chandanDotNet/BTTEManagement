using BTTEM.MediatR.CompanyProfile.Commands;
using BTTEM.Repository;
using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    public class GetAllCompanyAccountQueryHandler : IRequestHandler<GetAllCompanyAccountQuery, CompanyAccountList>
    {
        private readonly ICompanyAccountRepository _companyAccountRepository;
        public GetAllCompanyAccountQueryHandler(ICompanyAccountRepository companyAccountRepository)
        {
            _companyAccountRepository = companyAccountRepository;
        }
        public async Task<CompanyAccountList> Handle(GetAllCompanyAccountQuery request, CancellationToken cancellationToken)
        {
            return await _companyAccountRepository.GetCompanyAccounts(request.CompanyAccountResource);
        }
    }
}
