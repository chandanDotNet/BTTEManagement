using BTTEM.Data;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class CompanyGSTRepository
        : GenericRepository<CompanyGST, POSDbContext>, ICompanyGSTRepository
    {
        public CompanyGSTRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
