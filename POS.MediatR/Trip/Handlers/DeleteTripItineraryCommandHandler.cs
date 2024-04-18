using AutoMapper;
using BTTEM.MediatR.PoliciesTravel.Commands;
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
    public class DeleteTripItineraryCommandHandler : IRequestHandler<DeleteTripItineraryCommand, ServiceResponse<bool>>
    {
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTripItineraryCommandHandler> _logger;

        public DeleteTripItineraryCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteTripItineraryCommandHandler> logger
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(DeleteTripItineraryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _tripItineraryRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Trip Itinerary Does not exists");
                return ServiceResponse<bool>.Return404("Trip Itinerary  Does not exists");
            }

            //var exitingProduct = _productRepository.AllIncluding(c => c.Brand).Any(c => c.Brand.Id == entityExist.Id);
            //if (exitingProduct)
            //{
            //    _logger.LogError("Brand can not be Deleted because it is use in product");
            //    return ServiceResponse<bool>.Return409("Brand can not be Deleted because it is use in product.");
            //}

            _tripItineraryRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
