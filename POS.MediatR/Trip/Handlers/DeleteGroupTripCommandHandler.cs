using AutoMapper;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class DeleteGroupTripCommandHandler : IRequestHandler<DeleteGroupTripCommand, ServiceResponse<bool>>
    {
        private readonly IGroupTripRepository _groupTripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteGroupTripCommandHandler> _logger;
        public DeleteGroupTripCommandHandler(
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteGroupTripCommandHandler> logger,
           IGroupTripRepository groupTripRepository)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _groupTripRepository = groupTripRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteGroupTripCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _groupTripRepository.FindAsync(request.Id);

            if (entityExist == null)
            {
                _logger.LogError("Data does not exists");
                return ServiceResponse<bool>.Return404("Data does not exists");
            }
            _groupTripRepository.Remove(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
