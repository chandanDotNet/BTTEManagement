using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Branch.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Branch.Handler
{
    public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, ServiceResponse<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddBranchCommandHandler> _logger;
        private readonly IBranchRepository _branchRepository;
        public DeleteBranchCommandHandler(IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddBranchCommandHandler> logger,
            IBranchRepository branchRepository)
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _branchRepository = branchRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _branchRepository.All.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (entity == null)
            {
                _logger.LogError("Branch name not exist");
                return ServiceResponse<bool>.Return500();
            }           
            _branchRepository.Delete(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving branch name");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
