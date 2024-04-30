using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
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
    public class AddEmpGradeCommandHandler : IRequestHandler<AddEmpGradeCommand, ServiceResponse<EmpGradeDto>>
    {

        private readonly IEmpGradeRepository _empGradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddEmpGradeCommandHandler> _logger;

        public AddEmpGradeCommandHandler(
           IEmpGradeRepository empGradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddEmpGradeCommandHandler> logger
          )
        {
            _empGradeRepository = empGradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }


        public async Task<ServiceResponse<EmpGradeDto>> Handle(AddEmpGradeCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<BTTEM.Data.EmpGrade>(request);

            if (!string.IsNullOrWhiteSpace(request.GradeName))
            {

                var id = Guid.NewGuid();
                entity.Id = id;
                entity.IsActive = true;
            }

            _empGradeRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Grade");
                return ServiceResponse<EmpGradeDto>.Return500();
            }

            var industrydto = _mapper.Map<EmpGradeDto>(entity);
            return ServiceResponse<EmpGradeDto>.ReturnResultWith200(industrydto);
        }

    }
}
