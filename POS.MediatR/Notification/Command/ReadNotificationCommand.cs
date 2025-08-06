using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Notification.Command
{
    public class ReadNotificationCommand :IRequest<ServiceResponse<bool>>
    {
        //public List<Guid> Ids { get; set; }

        public Guid Id { get; set; }
    }
}
