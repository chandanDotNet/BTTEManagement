using AutoMapper;
using BTTEM.MediatR.TravelDocument.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.TravelDocument.Handlers
{
    public class UpdateTravelDocumentStatusCommandHandler : IRequestHandler<UpdateTravelDocumentStatusCommand, ServiceResponse<bool>>
    {

        private readonly ITravelDocumentRepository _travelDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTravelDocumentStatusCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateTravelDocumentStatusCommandHandler(
            ITravelDocumentRepository travelDocumentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateTravelDocumentStatusCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDocumentRepository = travelDocumentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTravelDocumentStatusCommand request, CancellationToken cancellationToken)
        {
            // var entity = _mapper.Map<Data.TravelDocument>(request);
            var entity = await _travelDocumentRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();           
            entity.IsVerified = request.IsVerified;  

            _travelDocumentRepository.Update(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
