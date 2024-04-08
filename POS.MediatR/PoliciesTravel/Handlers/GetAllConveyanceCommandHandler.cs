using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
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
    public class GetAllConveyanceCommandHandler : IRequestHandler<GetAllConveyanceCommand, List<ConveyanceDto>>
    {

        private readonly IConveyanceRepository _conveyanceRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;


        public GetAllConveyanceCommandHandler(
           IConveyanceRepository conveyanceRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper

          )
        {
            _conveyanceRepository = conveyanceRepository;
            _uow = uow;
            _mapper = mapper;

        }

        public async Task<List<ConveyanceDto>> Handle(GetAllConveyanceCommand request, CancellationToken cancellationToken)
        {

            //return await _travelsModeRepository.AllIncluding(c => c.classOfTravels).ProjectTo<TravelModeDto>(_mapper.ConfigurationProvider).ToListAsync();
            //var aa= await _travelsModeRepository.AllIncluding(c => c.classOfTravels).ToListAsync();

            var entities = await _conveyanceRepository.AllIncluding(c => c.conveyancesItem).ToListAsync();
            return _mapper.Map<List<ConveyanceDto>>(entities);

            //return aa.ToDynamicListAsync();
            //List<DepartmentDto> result = new List<DepartmentDto>(new List<DepartmentDto>());
            //if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            //{
            //    result =
            //}
            //else
            //{
            //    result = await _departmentRepository.All.Where(c => c.Id == request.Id).OrderBy(c => c.DepartmentName).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync();

            //}

            // return result;
        }

    }
}
