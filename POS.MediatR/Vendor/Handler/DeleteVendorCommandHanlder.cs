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

namespace BTTEM.MediatR.Handler
{
    public class DeleteVendorCommandHanlder : IRequestHandler<DeleteVendorCommand, ServiceResponse<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        private readonly ILogger<DeleteVendorCommandHanlder> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteVendorCommandHanlder(IMapper mapper, ILogger<DeleteVendorCommandHanlder> logger,
            IUnitOfWork<POSDbContext> uow, IVendorRepository vendorRepository)
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _vendorRepository = vendorRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _vendorRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Vendor not exist.");
                return ServiceResponse<bool>.Return409("Vendor not exist.");
            }
            var entity = _mapper.Map<Data.Vendor>(entityExist);
            _vendorRepository.Delete(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<bool>.Return500();
            }
            var returnEntity = _mapper.Map<VendorDto>(entity);
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
