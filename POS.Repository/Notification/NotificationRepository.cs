using BTTEM.Data;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class NotificationRepository : GenericRepository<Notification, POSDbContext>, INotificationRepository
    {
        public NotificationRepository(IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {

        }
    }
}
