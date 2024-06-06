using AutoMapper;
using BTTEM.Data;
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
    public class AddBranchCommandHandler : IRequestHandler<AddBranchCommand, ServiceResponse<BranchDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddBranchCommandHandler> _logger;
        private readonly IBranchRepository _branchRepository;
        public AddBranchCommandHandler(IMapper mapper, 
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddBranchCommandHandler> logger,
            IBranchRepository branchRepository)
        {
            _mapper= mapper;
            _uow= uow;
            _logger= logger;
            _branchRepository= branchRepository;
        }
        public async Task<ServiceResponse<BranchDto>> Handle(AddBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _branchRepository.All.Where(x => x.Name == request.Name).FirstOrDefaultAsync();
            if (entity != null)
            {
                _logger.LogError("Branch name already exist");
                return ServiceResponse<BranchDto>.Return500();
            }
            entity = _mapper.Map<Data.Branch>(request);
            entity.Id = Guid.NewGuid();
            _branchRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving branch name");
                return ServiceResponse<BranchDto>.Return500();
            }
            var entityDto = _mapper.Map<BranchDto>(entity);
            return ServiceResponse<BranchDto>.ReturnResultWith200(entityDto);
        }
    }
}
