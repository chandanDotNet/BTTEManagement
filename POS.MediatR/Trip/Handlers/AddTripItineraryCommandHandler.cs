using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class AddTripItineraryCommandHandler : IRequestHandler<AddTripItineraryCommand, ServiceResponse<TripItineraryDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTripItineraryCommandHandler> _logger;
        private readonly ITripRepository _tripRepository;

        public AddTripItineraryCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddTripItineraryCommandHandler> logger,
           IUserRepository userRepository,
           UserInfoToken userInfoToken,
           ITripRepository tripRepository
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _userInfoToken = userInfoToken;
            _tripRepository = tripRepository;
        }

        public async Task<ServiceResponse<TripItineraryDto>> Handle(AddTripItineraryCommand request, CancellationToken cancellationToken)
        {

            var userDetails = await _userRepository.AllIncluding(x => x.UserRoles).Where(u => u.Id == Guid.Parse(_userInfoToken.Id)).FirstOrDefaultAsync();

            foreach (var tv in request.TripItinerary)
            {
                var entity = _mapper.Map<Data.TripItinerary>(tv);
                entity.Id = Guid.NewGuid();
                entity.ApprovalStatus = "PENDING";
                if (userDetails.IsDirector && entity.CreatedBy == userDetails.Id)
                {
                    entity.ApprovalStatus = "APPROVED";
                    entity.ApprovalStatusDate = DateTime.Now;
                }

                //var roleId =  _userRepository.AllIncluding(x => x.UserRoles).Where(u => u.Id == Guid.Parse(_userInfoToken.Id)).FirstOrDefault()
                //    .UserRoles.FirstOrDefault().RoleId;

                if (userDetails.UserRoles.FirstOrDefault().RoleId == Guid.Parse("F72616BE-260B-41BB-A4EE-89146622179A"))
                {
                    entity.ApprovalStatus = "APPROVED";
                    entity.ApprovalStatusDate = DateTime.Now;
                }

                _tripItineraryRepository.Add(entity);
            }
            if (userDetails.UserRoles.FirstOrDefault().RoleId == Guid.Parse("F72616BE-260B-41BB-A4EE-89146622179A"))
            {
                var existEntity = await _tripRepository.FindAsync(request.TripItinerary.FirstOrDefault().TripId);
                existEntity.TripEnds = request.TripItinerary.FirstOrDefault().DepartureDate;
                _tripRepository.Update(existEntity);
            }else
            {
                var existEntity = await _tripRepository.FindAsync(request.TripItinerary.FirstOrDefault().TripId);
                if(existEntity.IsTripEndNotConfirmed==true)
                {
                    existEntity.Approval = "PENDING";
                    _tripRepository.Update(existEntity);
                }
            }

            //var entity = _mapper.Map<BTTEM.Data.TripItinerary>(request);            

            //    var id = Guid.NewGuid();
            //    entity.Id = id;

            //_tripItineraryRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Trip Itinerary");
                return ServiceResponse<TripItineraryDto>.Return500();
            }
            var entityRes = await _tripItineraryRepository.AllIncluding(c => c.ArrivalCity, b => b.DepartureCity).ProjectTo<TripItineraryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var tripItinerary = _mapper.Map<TripItineraryDto>(entityRes);
            return ServiceResponse<TripItineraryDto>.ReturnResultWith200(tripItinerary);
        }
    }
}
