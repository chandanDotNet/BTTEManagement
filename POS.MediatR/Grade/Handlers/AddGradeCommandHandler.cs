using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class AddGradeCommandHandler : IRequestHandler<AddGradeCommand, ServiceResponse<GradeDto>>
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddGradeCommandHandler> _logger;

        public AddGradeCommandHandler(
           IGradeRepository gradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddGradeCommandHandler> logger
          )
        {
            _gradeRepository = gradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
           
        }


        public async Task<ServiceResponse<GradeDto>> Handle(AddGradeCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<BTTEM.Data.Grade>(request);

            if (!string.IsNullOrWhiteSpace(request.GradeName))
            {
                
                var id = Guid.NewGuid();
                entity.Id = id;
            }

            _gradeRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Grade");
                return ServiceResponse<GradeDto>.Return500();
            }

            var industrydto = _mapper.Map<GradeDto>(entity);
            return ServiceResponse<GradeDto>.ReturnResultWith200(industrydto);
        }

    }
}
