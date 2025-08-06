using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AllNotificationsMarkAsReadCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid UserId { get; set; }
    }
}
