using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.TripTracking.Handler
{
    public class AddTripTrackingCommandHandler : IRequestHandler<AddTripTrackingCommand, ServiceResponse<TripTrackingDto>>
    {
        private readonly ITripTrackingRepository _tripTrackingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTripTrackingCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnviromant;
        private readonly PathHelper _pathHelper;

        public AddTripTrackingCommandHandler(
           ITripTrackingRepository tripTrackingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddTripTrackingCommandHandler> logger,
           IWebHostEnvironment webHostEnviromant, PathHelper pathHelper
          )
        {
            _tripTrackingRepository = tripTrackingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnviromant = webHostEnviromant;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<TripTrackingDto>> Handle(AddTripTrackingCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.TripTracking>(request);
            var id = Guid.NewGuid();
            entity.Id = id;

            _tripTrackingRepository.Add(entity);

            if (!string.IsNullOrEmpty(request.ImageUrlData))
            {
                entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip");
                return ServiceResponse<TripTrackingDto>.Return500();
            }
            if (!string.IsNullOrEmpty(request.ImageUrlData))
            {
                var pathToSave = Path.Combine(_webHostEnviromant.WebRootPath, _pathHelper.TrackingDocument);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

               await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            }

            if (!string.IsNullOrEmpty(request.ImageUrlData))
            {
                entity.ImageUrl = Path.Combine(_pathHelper.TrackingDocument, entity.ImageUrl);
            }
            var tripTracking = _mapper.Map<TripTrackingDto>(entity);
            return ServiceResponse<TripTrackingDto>.ReturnResultWith200(tripTracking);
        }



    }
}
