using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class AddTravelModeCommandHandler : IRequestHandler<AddTravelModeCommand, ServiceResponse<TravelModeDto>>
    {

        private readonly ITravelsModeRepository _travelsModeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddTravelModeCommandHandler> _logger;
        public AddTravelModeCommandHandler(
           ITravelsModeRepository travelsModeRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddTravelModeCommandHandler> logger
            )
        {
            _travelsModeRepository = travelsModeRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }



        public async Task<ServiceResponse<TravelModeDto>> Handle(AddTravelModeCommand request, CancellationToken cancellationToken)
        {
            //var entityExist = await _travelsModeRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            //if (entityExist != null)
            //{
            //    _logger.LogError("Policies Name already exist.");
            //    return ServiceResponse<TravelModeDto>.Return409("Policies Name already exist.");
            //}
            foreach (var tv in request.TravelModeData)
            {
                var entity = _mapper.Map<Data.TravelMode>(tv);
                entity.Id = Guid.NewGuid();

                _travelsModeRepository.Add(entity);
            }

            //var entity = _mapper.Map<Data.TravelMode>(request);
            //entity.Id = Guid.NewGuid();

            //_travelsModeRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<TravelModeDto>.Return500();
            }

            var entities = await _travelsModeRepository.AllIncluding(c => c.classOfTravels).ToListAsync();
            var entityDto = _mapper.Map<List<TravelModeDto>>(entities);
            //var entityDto = _mapper.Map<TravelModeDto>(entity);
            //return ServiceResponse<TravelModeDto>.ReturnResultWith200(entityDto);
            return ServiceResponse<TravelModeDto>.ReturnSuccess();
        }
    }
}
