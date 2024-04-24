using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class GetAllPoliciesSettingCommandHandler : IRequestHandler<GetAllPoliciesSettingCommand, List<PoliciesSettingDto>>
    {
        private readonly IPoliciesSettingRepository _policiesSettingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
      
        public GetAllPoliciesSettingCommandHandler(
            IPoliciesSettingRepository policiesSettingRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken
           
            )
        {
            _policiesSettingRepository = policiesSettingRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
           
        }


        public async Task<List<PoliciesSettingDto>> Handle(GetAllPoliciesSettingCommand request, CancellationToken cancellationToken)
        {

           

            var result = await _policiesSettingRepository.All.Where(c => c.PoliciesDetailId == request.Id).ProjectTo<PoliciesSettingDto>(_mapper.ConfigurationProvider).ToListAsync();


            return result;
        }

    }
}
