using BTTEM.Data.Entities.Expense;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class ExpenseDocumentRepository : GenericRepository<ExpenseDocument, POSDbContext>,
          IExpenseDocumentRepository
    {

        public ExpenseDocumentRepository(
           IUnitOfWork<POSDbContext> uow
           ) : base(uow)
        {
        }
    }
}
