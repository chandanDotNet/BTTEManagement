using BTTEM.Data;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class ExpenseCategoryTaxRepository : GenericRepository<ExpenseCategoryTax, POSDbContext>, IExpenseCategoryTaxRepository
    {
        public ExpenseCategoryTaxRepository(IUnitOfWork<POSDbContext> uow) : base(uow)
        {

        }
    }
}
