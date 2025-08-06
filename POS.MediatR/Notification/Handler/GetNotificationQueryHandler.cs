using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.ContactSupport.Handler;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Notification.Handler
{
    public class GetNotificationQueryHandler : IRequestHandler<GetNotificationQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetNotificationQueryHandler> _logger;

        public GetNotificationQueryHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<GetNotificationQueryHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<NotificationDto>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.All.Where(a => a.UserId == request.UserId && a.Read == 0).Include(u => u.SourceUser)
                .ToListAsync();
            if (notifications == null)
            {
                _logger.LogError("Notification not found");
            }
            var notificationDto = _mapper.Map<List<NotificationDto>>(notifications);
            return notificationDto;
        }
    }
}