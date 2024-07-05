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
    public class CarBikeLogBookExpenseDocumentRepository : GenericRepository<CarBikeLogBookExpenseDocument, POSDbContext>, ICarBikeLogBookExpenseDocumentRepository
    {

        public CarBikeLogBookExpenseDocumentRepository(
      IUnitOfWork<POSDbContext> uow
      ) : base(uow)
        {
        }
    }
}
