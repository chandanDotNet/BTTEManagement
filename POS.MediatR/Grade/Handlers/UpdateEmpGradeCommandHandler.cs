using AutoMapper;
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

namespace BTTEM.MediatR.Handlers
{
    public class UpdateEmpGradeCommandHandler : IRequestHandler<UpdateEmpGradeCommand, ServiceResponse<bool>>
    {

        private readonly IEmpGradeRepository _empGradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEmpGradeCommandHandler> _logger;

        public UpdateEmpGradeCommandHandler(
           IEmpGradeRepository empGradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateEmpGradeCommandHandler> logger
          )
        {
            _empGradeRepository = empGradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(UpdateEmpGradeCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _empGradeRepository
                .All.FirstOrDefaultAsync(c => c.GradeName == request.GradeName && c.Id != request.Id);

            if (entityExist != null)
            {
                _logger.LogError("Grade Already Exist.");
                return ServiceResponse<bool>.Return409("Grade Already Exist.");
            }
            entityExist = await _empGradeRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.GradeName = request.GradeName;
            entityExist.Description = request.Description;
            entityExist.IsActive = request.IsActive;
            _empGradeRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
