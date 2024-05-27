using BTTEM.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery

{
    public class GetNotificationQuery : IRequest<List<NotificationDto>>
    {
        public Guid? UserId { get; set; }
    }
}

