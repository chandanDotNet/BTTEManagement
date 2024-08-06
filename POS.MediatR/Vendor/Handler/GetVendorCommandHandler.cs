using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using BTTEM.Data.Dto;
using POS.Helper;
using POS.MediatR.Brand.Command;
using POS.MediatR.Brand.Handler;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.MediatR.Command;
using BTTEM.Repository;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace BTTEM.MediatR.Handler
{
    public class GetVendorCommandHandler : IRequestHandler<GetVendorCommand, ServiceResponse<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVendorCommandHandler> _logger;
        public GetVendorCommandHandler(
           IVendorRepository vendorRepository,
            IMapper mapper,
            ILogger<GetVendorCommandHandler> logger
            )
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<VendorDto>> Handle(GetVendorCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _vendorRepository.FindBy(c => c.Id == request.Id)
                .ProjectTo<VendorDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Vendor is not exists");
                return ServiceResponse<VendorDto>.Return404();
            }
            return ServiceResponse<VendorDto>.ReturnResultWith200(entityDto);
        }
    }
}
