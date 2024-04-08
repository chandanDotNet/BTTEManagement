using AutoMapper;
using BTTEM.MediatR.Grade.Commands;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.City.Commands;
using POS.MediatR.City.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Handlers
{
    public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, ServiceResponse<bool>>
    {

        private readonly IGradeRepository _gradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteGradeCommandHandler> _logger;

        public DeleteGradeCommandHandler(
           IGradeRepository gradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteGradeCommandHandler> logger
          )
        {
            _gradeRepository = gradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _gradeRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _gradeRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
