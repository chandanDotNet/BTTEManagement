using AutoMapper;
using BTTEM.MediatR.Grade.Commands;
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

namespace BTTEM.MediatR.Grade.Handlers
{
    public class DeleteEmpGradeCommandHandler : IRequestHandler<DeleteEmpGradeCommand, ServiceResponse<bool>>
    {

        private readonly IEmpGradeRepository _empGradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteEmpGradeCommandHandler> _logger;

        public DeleteEmpGradeCommandHandler(
           IEmpGradeRepository empGradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<DeleteEmpGradeCommandHandler> logger
          )
        {
            _empGradeRepository = empGradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<bool>> Handle(DeleteEmpGradeCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _empGradeRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _empGradeRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
