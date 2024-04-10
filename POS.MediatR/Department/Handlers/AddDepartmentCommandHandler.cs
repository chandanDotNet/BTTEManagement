using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Department.Commands;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
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

namespace BTTEM.MediatR.Handlers
{
    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, ServiceResponse<DepartmentDto>>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddDepartmentCommandHandler> _logger;

        public AddDepartmentCommandHandler(
           IDepartmentRepository departmentRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddDepartmentCommandHandler> logger
          )
        {
            _departmentRepository = departmentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }


        public async Task<ServiceResponse<DepartmentDto>> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<BTTEM.Data.Department>(request);

            if (!string.IsNullOrWhiteSpace(request.DepartmentName))
            {

                var id = Guid.NewGuid();
                entity.Id = id;
                entity.IsDeleted = false;
                entity.IsActive = true;
            }

            _departmentRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Grade");
                return ServiceResponse<DepartmentDto>.Return500();
            }

            var industrydto = _mapper.Map<DepartmentDto>(entity);
            return ServiceResponse<DepartmentDto>.ReturnResultWith200(industrydto);
        }

    }
}
