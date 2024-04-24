using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
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
    public class AddPoliciesSettingCommandHandler : IRequestHandler<AddPoliciesSettingCommand, ServiceResponse<PoliciesSettingDto>>
    {

        private readonly IPoliciesSettingRepository _policiesSettingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddPoliciesSettingCommandHandler> _logger;
        public AddPoliciesSettingCommandHandler(
            IPoliciesSettingRepository policiesSettingRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddPoliciesSettingCommandHandler> logger
            )
        {
            _policiesSettingRepository = policiesSettingRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<PoliciesSettingDto>> Handle(AddPoliciesSettingCommand request, CancellationToken cancellationToken)
        {
           
            var entity = _mapper.Map<Data.PoliciesSetting>(request);
            entity.Id = Guid.NewGuid();            
            entity.IsDeleted = false;            

            _policiesSettingRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PoliciesSettingDto>.Return500();
            }
            var entityDto = _mapper.Map<PoliciesSettingDto>(entity);
            return ServiceResponse<PoliciesSettingDto>.ReturnResultWith200(entityDto);
        }

    }
}
