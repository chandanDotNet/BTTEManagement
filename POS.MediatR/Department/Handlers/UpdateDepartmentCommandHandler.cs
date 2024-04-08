using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Department.Commands;
using BTTEM.MediatR.Grade.Commands;
using BTTEM.MediatR.Handlers;
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

namespace BTTEM.MediatR.Department.Handlers
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, ServiceResponse<bool>>
    {

        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDepartmentCommandHandler> _logger;

        public UpdateDepartmentCommandHandler(
           IDepartmentRepository departmentRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateDepartmentCommandHandler> logger
          )
        {
            _departmentRepository = departmentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }


        public async Task<ServiceResponse<bool>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _departmentRepository
                .All.FirstOrDefaultAsync(c => c.DepartmentName == request.DepartmentName && c.Id != request.Id);

            if (entityExist != null)
            {
                _logger.LogError("Department Already Exist.");
                return ServiceResponse<bool>.Return409("Department Already Exist.");
            }
            entityExist = await _departmentRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.DepartmentName = request.DepartmentName;
            entityExist.DepartmentCode = request.DepartmentCode;
            entityExist.DepartmentHeadId = request.DepartmentHeadId;
            entityExist.IsActive = request.IsActive;
            _departmentRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
