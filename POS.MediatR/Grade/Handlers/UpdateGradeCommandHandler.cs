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
using POS.MediatR.City.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Handlers
{
    public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, ServiceResponse<bool>>
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateGradeCommandHandler> _logger;

        public UpdateGradeCommandHandler(
           IGradeRepository gradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateGradeCommandHandler> logger
          )
        {
            _gradeRepository = gradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }


        public async Task<ServiceResponse<bool>> Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _gradeRepository
                .All.FirstOrDefaultAsync(c => c.GradeName == request.GradeName &&  c.Id != request.Id);

            if (entityExist != null)
            {
                _logger.LogError("Grade Already Exist.");
                return ServiceResponse<bool>.Return409("Grade Already Exist.");
            }
            entityExist = await _gradeRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.GradeName = request.GradeName;
            entityExist.Description = request.Description;
            entityExist.IsActive = request.IsActive;
            _gradeRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
