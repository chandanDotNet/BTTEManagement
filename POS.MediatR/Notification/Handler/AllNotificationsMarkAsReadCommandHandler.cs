using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class AllNotificationsMarkAsReadCommandHandler : IRequestHandler<AllNotificationsMarkAsReadCommand, ServiceResponse<bool>>
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<AllNotificationsMarkAsReadCommandHandler> _logger;

        public AllNotificationsMarkAsReadCommandHandler(IUnitOfWork<POSDbContext> uow,
            IMapper mapper, INotificationRepository notificationRepository,
            ILogger<AllNotificationsMarkAsReadCommandHandler> logger
            )
        {
            _uow = uow;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(AllNotificationsMarkAsReadCommand request, CancellationToken cancellationToken)
        {

            var entityExist = await _notificationRepository.All.Where(v => v.UserId == request.UserId && v.Read == 0).ToListAsync();
            if (entityExist.Count() > 0)
            {
                entityExist.ForEach(item =>
                {
                    item.UserId = request.UserId;
                    item.Read = 1;
                });

                _notificationRepository.UpdateRange(entityExist);

                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while read all notification");
                    return ServiceResponse<bool>.Return500();
                }

                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            //result = _mapper.Map<bool>(result);
            return ServiceResponse<bool>.ReturnResultWith200(false);

        }
    }
}
