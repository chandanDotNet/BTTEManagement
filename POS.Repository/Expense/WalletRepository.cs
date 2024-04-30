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
    public class WalletRepository : GenericRepository<Wallet, POSDbContext>,
           IWalletRepository
    {

        public WalletRepository(
           IUnitOfWork<POSDbContext> uow
           ) : base(uow)
        {
        }
    }
}
