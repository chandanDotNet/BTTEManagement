using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddNotificationCommand : IRequest<ServiceResponse<NotificationDto>>
    {
        public Guid UserId { get; set; }
        public Guid SourceId { get; set; }
        public string Content { get; set; }
        public int Read { get; set; }
    }
}
