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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, ServiceResponse<bool>>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTripCommandHandler> _logger;

        public DeleteTripCommandHandler(
           ITripRepository tripRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteTripCommandHandler> logger
          )
        {
            _tripRepository = tripRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _tripRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Trip Does not exists");
                return ServiceResponse<bool>.Return404("Trip Does not exists");
            }

            //var exitingProduct = _productRepository.AllIncluding(c => c.Brand).Any(c => c.Brand.Id == entityExist.Id);
            //if (exitingProduct)
            //{
            //    _logger.LogError("Brand can not be Deleted because it is use in product");
            //    return ServiceResponse<bool>.Return409("Brand can not be Deleted because it is use in product.");
            //}

            _tripRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
