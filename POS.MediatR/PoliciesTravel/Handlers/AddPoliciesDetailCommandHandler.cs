using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class AddPoliciesDetailCommandHandler : IRequestHandler<AddPoliciesDetailCommand, ServiceResponse<PoliciesDetailDto>>
    {


        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddPoliciesDetailCommandHandler> _logger;
        public AddPoliciesDetailCommandHandler(
           IPoliciesDetailRepository policiesDetailRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddPoliciesDetailCommandHandler> logger
            )
        {
            _policiesDetailRepository = policiesDetailRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }



        public async Task<ServiceResponse<PoliciesDetailDto>> Handle(AddPoliciesDetailCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _policiesDetailRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Policies Name already exist.");
                return ServiceResponse<PoliciesDetailDto>.Return409("Policies Name already exist.");
            }

            var entity = _mapper.Map<Data.PoliciesDetail>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedBy = Guid.Parse(_userInfoToken.Id);
            //entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedBy = Guid.Parse(_userInfoToken.Id);

            _policiesDetailRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PoliciesDetailDto>.Return500();
            }
            var entityDto = _mapper.Map<PoliciesDetailDto>(entity);
            return ServiceResponse<PoliciesDetailDto>.ReturnResultWith200(entityDto);
        }
    }
}
