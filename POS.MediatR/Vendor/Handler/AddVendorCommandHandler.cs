using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using SixLabors.ImageSharp.Formats.Gif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handler
{
    public class AddVendorCommandHandler : IRequestHandler<AddVendorCommand, ServiceResponse<VendorDto>>
    {
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        private readonly ILogger<AddVendorCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public AddVendorCommandHandler(IMapper mapper, ILogger<AddVendorCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow, IVendorRepository vendorRepository)
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _vendorRepository = vendorRepository;
        }

        public async Task<ServiceResponse<VendorDto>> Handle(AddVendorCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _vendorRepository.FindBy(v => v.VendorName == request.VendorName).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Vendor already exist.");
                return ServiceResponse<VendorDto>.Return409("Vendor already exist.");
            }
            request.Id = Guid.NewGuid();
            var entity = _mapper.Map<Vendor>(request);
            _vendorRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<VendorDto>.Return500();
            }
            var returnEntity = _mapper.Map<VendorDto>(entity);
            return ServiceResponse<VendorDto>.ReturnResultWith200(returnEntity);
        }
    }
}
