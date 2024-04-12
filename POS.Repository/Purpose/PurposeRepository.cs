using BTTEM.Data;
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
    public class PurposeRepository : GenericRepository<Purpose, POSDbContext>, IPurposeRepository
    {

        public PurposeRepository(
           IUnitOfWork<POSDbContext> uow
           ) : base(uow)
        {
        }
    }
}
