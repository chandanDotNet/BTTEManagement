using POS.Common.GenericRepository;
using POS.Data.Resources;
using POS.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data;
using BTTEM.Data.Resources;

namespace BTTEM.Repository
{
    public interface ICompanyAccountRepository : IGenericRepository<CompanyAccount>
    {
        Task<CompanyAccountList> GetCompanyAccounts(CompanyAccountResource companyAccountResource);
    }
}
