using AutoMapper;
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
    public class GetAllPoliciesLodgingFoodingCommandHandler : IRequestHandler<GetAllPoliciesLodgingFoodingCommand, PoliciesLodgingFoodingDto>
    {

        private readonly IPoliciesLodgingFoodingRepository _policiesLodgingFoodingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;


        public GetAllPoliciesLodgingFoodingCommandHandler(
           IPoliciesLodgingFoodingRepository policiesLodgingFoodingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper

          )
        {
            _policiesLodgingFoodingRepository = policiesLodgingFoodingRepository;
            _uow = uow;
            _mapper = mapper;

        }


        public async Task<PoliciesLodgingFoodingDto> Handle(GetAllPoliciesLodgingFoodingCommand request, CancellationToken cancellationToken)
        {
                        

            var entities = await _policiesLodgingFoodingRepository.All.Where(c => c.PoliciesDetailId== request.Id).FirstOrDefaultAsync();
            return _mapper.Map<PoliciesLodgingFoodingDto>(entities);

           
        }

    }
}
