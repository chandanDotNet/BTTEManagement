using AutoMapper;
using BTTEM.MediatR.Notification.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
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
    public class ReadNotificationCommandHandler : IRequestHandler<ReadNotificationCommand, ServiceResponse<bool>>
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _noticationRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<ReadNotificationCommandHandler> _logger;
        public ReadNotificationCommandHandler(IMapper mapper, INotificationRepository noticationRepository, IUnitOfWork<POSDbContext> uow,
            ILogger<ReadNotificationCommandHandler> logger)
        {
            _mapper = mapper;
            _noticationRepository = noticationRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Ids)
            {
                var entity = await _noticationRepository.FindAsync(item);
                if (entity == null)
                {
                    _logger.LogError("Data not found!");
                    return ServiceResponse<bool>.Return409("Data not found!");
                }
                entity.Read = 1;
                _noticationRepository.Update(entity);
            }
            if (await _uow.SaveAsync() <= 1)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
