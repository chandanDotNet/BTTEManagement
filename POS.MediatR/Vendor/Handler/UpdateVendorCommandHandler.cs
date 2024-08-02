using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Command;
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
using Microsoft.EntityFrameworkCore;

namespace BTTEM.MediatR.Handler
{
    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, ServiceResponse<VendorDto>>
    {
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        private readonly ILogger<UpdateVendorCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public UpdateVendorCommandHandler(IMapper mapper, ILogger<UpdateVendorCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow, IVendorRepository vendorRepository)
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _vendorRepository = vendorRepository;
        }

        public async Task<ServiceResponse<VendorDto>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _vendorRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Vendor not exist.");
                return ServiceResponse<VendorDto>.Return409("Vendor not exist.");
            }
            entityExist = _mapper.Map(request, entityExist);
            _vendorRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<VendorDto>.Return500();
            }
            var returnEntity = _mapper.Map<VendorDto>(entityExist);
            return ServiceResponse<VendorDto>.ReturnResultWith200(returnEntity);
        }
    }
}
