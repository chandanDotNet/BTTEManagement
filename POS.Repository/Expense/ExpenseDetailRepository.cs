using BTTEM.Data;
using BTTEM.Data.Entities.Expense;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository.Expense
{
    public class ExpenseDetailRepository : GenericRepository<ExpenseDetail, POSDbContext>,
          IExpenseDetailRepository
    {

        public ExpenseDetailRepository(
          IUnitOfWork<POSDbContext> uow
          ) : base(uow)
        {
        }
    }
}
