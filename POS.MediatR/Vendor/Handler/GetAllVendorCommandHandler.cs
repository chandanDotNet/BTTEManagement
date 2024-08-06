using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.Brand.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handler
{
    public class GetAllVendorCommandHandler : IRequestHandler<GetAllVendorCommand, List<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public GetAllVendorCommandHandler(
           IVendorRepository vendorRepository,
            IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorDto>> Handle(GetAllVendorCommand request, CancellationToken cancellationToken)
        {
            var entities = await _vendorRepository.All
                .Select(c => new VendorDto
                {
                    Id = c.Id,
                    VendorName = c.VendorName,
                    Email= c.Email,
                    GSTNo=c.GSTNo,
                    Phone=c.Phone,
                    ResponsiblePersonName=c.ResponsiblePersonName,
                    VendorAddress = c.VendorAddress,
                    VendorCode=c.VendorCode,
                    Website = c.Website
                }).ToListAsync();
            return entities;
        }
    }
}
