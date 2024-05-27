using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Notification.Handler
{
    internal class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand, ServiceResponse<NotificationDto>>
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<AddNotificationCommandHandler> _logger;

        public AddNotificationCommandHandler(IUnitOfWork<POSDbContext> uow,
            IMapper mapper, INotificationRepository notificationRepository,
            ILogger<AddNotificationCommandHandler> logger
            )
        {
            _uow = uow;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }
        public async Task<ServiceResponse<NotificationDto>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.Notification>(request);
            entity.Id = Guid.NewGuid();            

            _notificationRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving request call");
                return ServiceResponse<NotificationDto>.Return500();
            }
            var notificationDto = _mapper.Map<NotificationDto>(entity);
            return ServiceResponse<NotificationDto>.ReturnResultWith200(notificationDto);

        }
    }
}
