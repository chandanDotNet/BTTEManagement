using AutoMapper;
using BTTEM.MediatR.CompanyProfile.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.City.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    public class DeleteCompanyGSTCommandHandler : IRequestHandler<DeleteCompanyGSTCommand, ServiceResponse<bool>>
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ICompanyGSTRepository _stateWiseGSTReepository;
        private readonly ILogger<DeleteCompanyGSTCommand> _logger;
        public DeleteCompanyGSTCommandHandler(IUnitOfWork<POSDbContext> uow,
            IMapper mapper, ICompanyGSTRepository stateWiseGSTReepository,
            ILogger<DeleteCompanyGSTCommand> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _stateWiseGSTReepository = stateWiseGSTReepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCompanyGSTCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _stateWiseGSTReepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _stateWiseGSTReepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
