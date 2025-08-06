using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class TaxCodeRepository : GenericRepository<TaxCode, POSDbContext>, ITaxCodeRepository
    {
        public TaxCodeRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
