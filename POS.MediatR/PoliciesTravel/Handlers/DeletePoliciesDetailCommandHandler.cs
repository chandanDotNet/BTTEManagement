using AutoMapper;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Brand.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class DeletePoliciesDetailCommandHandler : IRequestHandler<DeletePoliciesDetailCommand, ServiceResponse<bool>>
    {
        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<DeletePoliciesDetailCommandHandler> _logger;
        public DeletePoliciesDetailCommandHandler(
           IPoliciesDetailRepository policiesDetailRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<DeletePoliciesDetailCommandHandler> logger
            )
        {
            _policiesDetailRepository = policiesDetailRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<bool>> Handle(DeletePoliciesDetailCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _policiesDetailRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Policies Detail Does not exists");
                return ServiceResponse<bool>.Return404("Policies Detail  Does not exists");
            }

            //var exitingProduct = _productRepository.AllIncluding(c => c.Brand).Any(c => c.Brand.Id == entityExist.Id);
            //if (exitingProduct)
            //{
            //    _logger.LogError("Brand can not be Deleted because it is use in product");
            //    return ServiceResponse<bool>.Return409("Brand can not be Deleted because it is use in product.");
            //}

            _policiesDetailRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Brand.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }

    }
}
