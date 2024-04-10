using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data.Dto;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.Department.Commands;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class GetAllTravelsModesCommandHandler : IRequestHandler<GetAllTravelsModesCommand, List<TravelModeDto>>
    {
        private readonly ITravelsModeRepository _travelsModeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;


        public GetAllTravelsModesCommandHandler(
           ITravelsModeRepository travelsModeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper

          )
        {
            _travelsModeRepository = travelsModeRepository;
            _uow = uow;
            _mapper = mapper;

        }

        public async Task<List<TravelModeDto>> Handle(GetAllTravelsModesCommand request, CancellationToken cancellationToken)
        {
                     

            //var entities = await _travelsModeRepository.AllIncluding(c => c.classOfTravels).ToListAsync();

            //entities = (List<Data.TravelMode>)entities.Where(c => c.PoliciesDetailId == request.Id);

            //return _mapper.Map<List<TravelModeDto>>(entities);


            List<TravelModeDto> result = new List<TravelModeDto>(new List<TravelModeDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                result = await _travelsModeRepository.AllIncluding(c => c.classOfTravels).Where(c=>c.IsMaster==true).ProjectTo<TravelModeDto>(_mapper.ConfigurationProvider).ToListAsync();
            }
            else
            {
                result = await _travelsModeRepository.AllIncluding(c => c.classOfTravels).Where(c => c.PoliciesDetailId == request.Id).OrderBy(c => c.TravelsModesName).ProjectTo<TravelModeDto>(_mapper.ConfigurationProvider).ToListAsync();
                if(result.Count ==0)
                {
                    result = await _travelsModeRepository.AllIncluding(c => c.classOfTravels).Where(c=> c.IsMaster==true).OrderBy(c => c.TravelsModesName).ProjectTo<TravelModeDto>(_mapper.ConfigurationProvider).ToListAsync();
                }

            }

            return result;

        }


    }
}
