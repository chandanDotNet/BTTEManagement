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
    public class DeleteTripHotelBookingCommandHandler : IRequestHandler<DeleteTripHotelBookingCommand, ServiceResponse<bool>>
    {

        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTripHotelBookingCommandHandler> _logger;

        public DeleteTripHotelBookingCommandHandler(
           ITripHotelBookingRepository tripHotelBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteTripHotelBookingCommandHandler> logger
          )
        {
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(DeleteTripHotelBookingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _tripHotelBookingRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Trip Hotel Booking Does not exists");
                return ServiceResponse<bool>.Return404("Trip Hotel Booking  Does not exists");
            }

            //var exitingProduct = _productRepository.AllIncluding(c => c.Brand).Any(c => c.Brand.Id == entityExist.Id);
            //if (exitingProduct)
            //{
            //    _logger.LogError("Brand can not be Deleted because it is use in product");
            //    return ServiceResponse<bool>.Return409("Brand can not be Deleted because it is use in product.");
            //}

            _tripHotelBookingRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Hotel Booking.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
