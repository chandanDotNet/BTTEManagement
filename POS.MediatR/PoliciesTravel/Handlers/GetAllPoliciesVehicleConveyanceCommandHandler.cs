using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class GetAllPoliciesVehicleConveyanceCommandHandler : IRequestHandler<GetAllPoliciesVehicleConveyanceCommand, List<PoliciesVehicleConveyanceDto>>
    {
        private readonly IPoliciesVehicleConveyanceRepository _policiesVehicleConveyanceRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;


        public GetAllPoliciesVehicleConveyanceCommandHandler(
           IPoliciesVehicleConveyanceRepository policiesVehicleConveyanceRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper

          )
        {
            _policiesVehicleConveyanceRepository = policiesVehicleConveyanceRepository;
            _uow = uow;
            _mapper = mapper;

        }


        public async Task<List<PoliciesVehicleConveyanceDto>> Handle(GetAllPoliciesVehicleConveyanceCommand request, CancellationToken cancellationToken)
        {

            //return await _travelsModeRepository.AllIncluding(c => c.classOfTravels).ProjectTo<TravelModeDto>(_mapper.ConfigurationProvider).ToListAsync();
            //var aa= await _travelsModeRepository.AllIncluding(c => c.classOfTravels).ToListAsync();

            //var entities = await _conveyanceRepository.AllIncluding(c => c.conveyancesItem).ToListAsync();
            //return _mapper.Map<List<ConveyanceDto>>(entities);

            //List<PoliciesVehicleConveyanceDto> result = new List<PoliciesVehicleConveyanceDto>(new List<PoliciesVehicleConveyanceDto>());
            //if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            //{
            //    result = await _policiesVehicleConveyanceRepository.AllIncluding(c => c.conveyancesItem).Where(c => c.IsMaster == true).ProjectTo<ConveyanceDto>(_mapper.ConfigurationProvider).ToListAsync();
            //}
            //else
            //{
            //    result = await _policiesVehicleConveyanceRepository.AllIncluding(c => c.conveyancesItem).Where(c => c.PoliciesDetailId == request.Id).OrderBy(c => c.Name).ProjectTo<ConveyanceDto>(_mapper.ConfigurationProvider).ToListAsync();
            //    if (result.Count == 0)
            //    {
            //        result = await _policiesVehicleConveyanceRepository.AllIncluding(c => c.conveyancesItem).Where(c => c.IsMaster == true).OrderBy(c => c.Name).ProjectTo<ConveyanceDto>(_mapper.ConfigurationProvider).ToListAsync();
            //    }

            //}

           var result = await _policiesVehicleConveyanceRepository.All.Where(c => c.PoliciesDetailId == request.Id).ProjectTo<PoliciesVehicleConveyanceDto>(_mapper.ConfigurationProvider).ToListAsync();


            return result;
        }


    }
}
