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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Handlers
{
    public class AddGradeListCommandHandler : IRequestHandler<AddGradeListCommand, ServiceResponse<bool>>
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddGradeCommandHandler> _logger;       
        public AddGradeListCommandHandler(IGradeRepository gradeRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<AddGradeCommandHandler> logger)
        {
            _gradeRepository = gradeRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(AddGradeListCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.GradeList)
            {
                var entityExist = _gradeRepository.All.Where(x => x.GradeName == item.GradeName).FirstOrDefault();
                if (entityExist == null)
                {
                    var entity = _mapper.Map<BTTEM.Data.Grade>(item);
                    entity.Id = Guid.NewGuid();
                    _gradeRepository.Add(entity);
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
