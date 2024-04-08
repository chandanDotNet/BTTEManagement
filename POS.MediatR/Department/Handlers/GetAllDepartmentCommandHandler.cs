using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Department.Commands;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.Brand.Command;
using POS.MediatR.Currency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Department.Handlers
{
    public class GetAllDepartmentCommandHandler : IRequestHandler<GetAllDepartmentCommand, List<DepartmentDto>>
    {

        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
    

        public GetAllDepartmentCommandHandler(
           IDepartmentRepository departmentRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper
          
          )
        {
            _departmentRepository = departmentRepository;
            _uow = uow;
            _mapper = mapper;          

        }

        public async Task<List<DepartmentDto>> Handle(GetAllDepartmentCommand request, CancellationToken cancellationToken)
        {
            List<DepartmentDto> result = new List<DepartmentDto>(new List<DepartmentDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                result = await _departmentRepository.All.OrderBy(c => c.DepartmentName).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync();
            }
            else
            {
                result = await _departmentRepository.All.Where(c => c.Id == request.Id).OrderBy(c => c.DepartmentName).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync();
                
            }

            return result;
        }
            

    }
}
