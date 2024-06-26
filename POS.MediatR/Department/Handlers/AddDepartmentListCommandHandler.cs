using AutoMapper;
using BTTEM.Data.Dto;
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

namespace BTTEM.MediatR.Department.Handlers
{
    public class AddDepartmentListCommandHandler : IRequestHandler<AddDepartmentListCommand, ServiceResponse<bool>>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddDepartmentListCommandHandler> _logger;

        public AddDepartmentListCommandHandler(
           IDepartmentRepository departmentRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddDepartmentListCommandHandler> logger
          )
        {
            _departmentRepository = departmentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(AddDepartmentListCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.DepartmentList)
            {
                var entityExist = _departmentRepository.All.Where(x => x.DepartmentName == item.DepartmentName).FirstOrDefault();
                if (entityExist == null)
                {
                    var entity = _mapper.Map<BTTEM.Data.Department>(item);
                    entity.Id = Guid.NewGuid();
                    entity.DepartmentHeadId = new Guid("4B352B37-332A-40C6-AB05-E38FCF109719");
                    _departmentRepository.Add(entity);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        _logger.LogError("Error while saving Grade");
                        return ServiceResponse<bool>.Return500();
                    }
                }
            }
            var dtoEntity = _mapper.Map<bool>(true);
            return ServiceResponse<bool>.ReturnResultWith200(dtoEntity);
        }
    }
}
