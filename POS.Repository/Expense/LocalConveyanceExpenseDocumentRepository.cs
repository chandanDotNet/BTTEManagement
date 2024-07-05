using BTTEM.Data;
using BTTEM.Data.Entities;
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
    public class LocalConveyanceExpenseDocumentRepository : GenericRepository<LocalConveyanceExpenseDocument, POSDbContext>, ILocalConveyanceExpenseDocumentRepository
    {


        public LocalConveyanceExpenseDocumentRepository(
        IUnitOfWork<POSDbContext> uow
        ) : base(uow)
        {
        }
    }
}
